using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using DB_s2_1_1.ViewModel;
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
    public class TrainsService : ITrainsService
    {
        private readonly TrainsContext _context;

        public TrainsService(TrainsContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TrainsViewModel>> GetPagedTrains(TrainsFilters trainsFilters, int page)
        {
            var predicate = trainsFilters.GetPredicate();
            var trainsContext = _context.Trains
                .AsNoTracking()
                .Where(trainsFilters.GetPredicate())
                .Select(e => new TrainsViewModel
                {
                    Id = e.Id,
                    Category = e.Category.Name,
                    SeatsQty = e.SeatsQty,
                    Station = e.Station.Name,
                    RouteId = e.RouteId
                });
            if (page == 0)
                page = 1;
            var res = await trainsContext.GetPaged(page);
            return res;
        }

        public async Task<TrainsIndex> GetTrainsIndex(TrainsIndex trainsIndex)
        {
            int page = trainsIndex.PagedTrains == null ? 1 : trainsIndex.PagedTrains.CurrentPage;
            TrainsFilters trainsFilters = trainsIndex.TrainsFilters ?? (new());

            return new()
            {
                PagedTrains = await GetPagedTrains(trainsFilters, page),
                TrainsFilters = trainsFilters,
                RouteIdList = await GetRoutesIdList(),
                Categories = GetCategoriesSelectList(trainsFilters.SearchCategory)
            };
        }

        public async Task<List<int>> GetRoutesIdList()
        {
            return await _context.Routes.AsNoTracking().Select(r => r.Id).ToListAsync();
        }

        public SelectList GetCategoriesSelectList(int? selectedCategory = null)
        {
            return new SelectList(_context.Categories.AsNoTracking(), "Id", "Name", selectedCategory);
        }

        public SelectList GetStationsSelectList(int? selectedStation = null)
        {
            return new SelectList(_context.Stations.AsNoTracking(), "Id", "Name", selectedStation);
        }

        public SelectList GetRoutesSelectList(int? selectedRoute = null)
        {
            return new SelectList(_context.Routes.AsNoTracking(), "Id", "Id", selectedRoute);
        }

        public async Task<Train> GetTrainNoTraking(int trainId)
        {
            return await _context.Trains.AsNoTracking().FirstOrDefaultAsync(t => t.Id == trainId);
        }

        public async Task<TrainsViewModel> GetTrainWithDetails(int id)
        {
            return await _context.Trains
                .AsNoTracking()
                .Include(t => t.Employees)
                .ThenInclude(empl => empl.Station)
                .Include(t => t.Route)
                .ThenInclude(r => r.Stations)
                .ThenInclude(sr => sr.Station)
                .Select(e => new TrainsViewModel
                {
                    Id = e.Id,
                    Category = e.Category.Name,
                    SeatsQty = e.SeatsQty,
                    Station = e.Station.Name,
                    Route = e.Route,
                    RouteId = e.RouteId,
                    Employees = e.Employees
                })
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public TrainEditing GetTrainEditing(TrainEditing trainsCreate = null)
        {
            return new()
            {
                Categories = GetCategoriesSelectList(trainsCreate?.Train?.CategoryId),
                Stations = GetStationsSelectList(trainsCreate?.Train?.StationId),
                Routes = GetRoutesSelectList(trainsCreate?.Train?.RouteId),
            };
        }

        public async Task<Train> GetTrain(int id)
        {
            return await _context.Trains
               .Include(t => t.Category)
               .Include(t => t.Station)
               .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<TrainEditing> GetTrainEditing(int trainId)
        {
            Train train = await GetTrainNoTraking(trainId);
            if (train == null)
                return null;
            return new()
            {
                Categories = GetCategoriesSelectList(train?.CategoryId),
                Stations = GetStationsSelectList(train?.StationId),
                Routes = GetRoutesSelectList(train?.RouteId),
                Train = train
            };
        }

        public async Task AddTrain(TrainEditing trainsCreate) //ДОПИСАТЬ
        {
            if (trainsCreate.Train == null)
                return;

            await _context.AddAsync(trainsCreate.Train);
            await _context.SaveChangesAsync();

        }

        public async Task<string> UpdateTrain(TrainEditing trainEditing)
        {
            try
            {
                _context.Update(trainEditing.Train);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return "Error writing to database.";
            }
            return null;
        }

        public async Task<TrainsEditBrigade> GetTrainsEditBrigade(int trainId, int page)
        {
            var train = await _context.Trains.Include(e => e.Employees)
               .ThenInclude(empl => empl.Station)
               .FirstOrDefaultAsync(e => e.Id == trainId);

            if (train == null)
                return null;

            return new()
            {
                Train = train,
                Empls = await _context.Employees.AsNoTracking()
                .Include(e => e.Station)
                .Include(e => e.Trains)
                .GetPaged(page)
            };
        }

        public async Task<string> UpdateTrainBrigade(TrainsEditBrigade trainsEditBrigade)
        {

            int trainId;
            if (trainsEditBrigade.Train == null || (trainId = trainsEditBrigade.GetTrainId()) < 1)
                return "No train information error";
            
            Train train = await _context.Trains.Include(t=> t.Employees).FirstOrDefaultAsync(t=> t.Id == trainId);
            if (train == null)
                return "No train information error";
            train.Employees.Clear();
            if (trainsEditBrigade.SelectedEmpls != null)
            {
                var empls = await _context.Employees.Where(e => trainsEditBrigade.SelectedEmpls.Contains(e.Id)).ToListAsync();
                foreach (Employee empl in empls)
                {
                    train.Employees.Add(empl);
                }
            }
            try
            {
                _context.Entry(train).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                return exc.InnerException?.Message ?? exc.Message;
            }
            return null;
        }


        public async Task RemoveTrain(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            _context.Trains.Remove(train);
            await _context.SaveChangesAsync();
        }
    }
}
