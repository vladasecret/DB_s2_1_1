using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using DB_s2_1_1.ViewModel;
using DB_s2_1_1.ViewModel.Timetables;
using DB_s2_1_1.ViewModel.Trains;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public class TimetablesService : ITimetablesService
    {
        private readonly TrainsContext _context;

        public TimetablesService(TrainsContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Timetable>> GetTimetablesIndex(int page)
        {
            return await _context.Timetables.AsNoTracking().Include(t => t.Station).Include(t => t.Train).GetPaged(page);
        }

        public async Task<Timetable> GetTimetableDetails(int id)
        {
            return await _context.Timetables.AsNoTracking()
                           .Include(t => t.Station)
                           .Include(t => t.Train)
                           .FirstOrDefaultAsync(m => m.Id == id);
        }

        public SelectList GetTrainsIdSelectList(int? selectedId = null)
        {
            return new SelectList(_context.Trains.AsNoTracking().Where(e => e.RouteId != null), "Id", "Id", selectedId);
        }

        public TimetablesCreateModel GetTimetablesCreateModel()
        {
            return new()
            {
                TrainsSelectList = GetTrainsIdSelectList()
            };
        }

        /*
        private DateTime GetLastDepartureTime(int trainId)
        {
            return _context.Timetables.AsNoTracking().Where(t => t.TrainId == trainId).Max(t => (DateTime?)t.DepartureTime) ?? DateTime.MinValue;
        }
        

        /*
        private int GetLastRoadId(int trainId)
        {
            return _context.Timetables.AsNoTracking().Where(t => t.TrainId == trainId).Max(t => t.RoadId);
        }
        */

        private async Task<Timetable> GetLastTimetable(int trainId)
        {

            return await _context.Timetables.AsNoTracking().Where(t => t.TrainId == trainId).OrderByDescending(t => t.DepartureTime).FirstOrDefaultAsync();
            
            
        }
        
        private List<int> GetOrderedRouteStationId (ICollection<RouteStation> routeStations, bool direction)
        {
            return direction ? routeStations.OrderBy(item => item.StationOrder).Select(item => item.StationId).ToList()
                : routeStations.OrderByDescending(item => item.StationOrder).Select(item => item.StationId).ToList();
        }

        private double GetDistance(int firstId, int secondId)
        {
            return _context.StationRoads.AsNoTracking()
                .Where(e => (e.FirstStationId == firstId && e.SecondStationId == secondId)
                || (e.FirstStationId == secondId && e.SecondStationId == firstId))
                .Select(e => e.Distance)
                .FirstOrDefault();
        }

        public async Task<string> GenerateTimetables(TimetablesCreateModel createModel)
        {
            Train train = await _context.Trains.AsNoTracking()
                .Include(t => t.Route)
                .ThenInclude(r => r.Stations)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == createModel.TrainId);

            if (train == null)
            {
                return "Train not found.";
            }

            Timetable lastTimetable = await GetLastTimetable(createModel.TrainId);
            DateTime LastDepartureTime = lastTimetable?.DepartureTime ?? DateTime.MinValue;

            if (lastTimetable?.TrainDirection == createModel.TrainDirection)
            {
                return "A train cannot travel twice in the same direction. Change the direction.";
            }

            if (LastDepartureTime.CompareTo(createModel.ArrivalTime) >= 0)
            {
                return $"Arrival time error. Arrival time must be greater than {LastDepartureTime}";
            }

            int roadId = (lastTimetable?.RoadId ?? 0) + 1;

            List<int> routeStationsId = GetOrderedRouteStationId(train.Route.Stations, createModel.TrainDirection);

            double speed = train.Category.Speed;
            if (speed <= 0)
                speed = 30;

            int prevStationId = 0;
            double distance = 0;
            DateTime departureTime = new();
            DateTime arrivalTime = createModel.ArrivalTime;
            Random rand = new();

            try
            {
                foreach (int stationId in routeStationsId)
                {
                    distance = GetDistance(prevStationId, stationId);
                    arrivalTime = arrivalTime.AddMinutes(distance / speed * 60);
                    departureTime = arrivalTime.AddMinutes(rand.Next(5, 60));
                    prevStationId = stationId;

                    _context.Timetables.Add(new Timetable
                    {
                        RoadId = roadId,
                        TrainId = createModel.TrainId,
                        StationId = stationId,
                        ArrivalTime = arrivalTime,
                        DepartureTime = departureTime,
                        TrainDirection = createModel.TrainDirection
                    });

                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return "Database writing exception.";
            }

            return null;
        }

    }
}
