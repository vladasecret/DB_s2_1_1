using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using DB_s2_1_1.ViewModel.Routes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace DB_s2_1_1.Services
{
    public class RoutesService : IRoutesService
    {
        private readonly TrainsContext _context;

        public RoutesService(TrainsContext context)
        {
            _context = context;
        }

        private async Task<PagedResult<Route>> GetPagedRoutes(int searchRoute, string searchCity, int page = 1)
        {
            var predicate = PredicateBuilder.New<Route>(true);

            if (searchRoute > 0)
            {
                predicate.And(item => item.Id == searchRoute);
            }

            if (!string.IsNullOrEmpty(searchCity))
            {
                predicate.And(item => item.Stations
                .Where(sr => sr.Station.City.ToLower().Contains(searchCity.ToLower()))
                .Any());
            }

            return await _context.Routes
                                .AsNoTracking()
                                .Include(r => r.Stations)
                                .ThenInclude(sr => sr.Station)
                                .Where(predicate)
                                .GetPaged(page);
        }

        public async Task<RoutesIndex> GetRoutesIndex(int searchRoute, string searchCity, int page = 1)
        {
            RoutesIndex res = new()
            {
                SearchRoute = searchRoute == 0 ? null : searchRoute,
                SearchCity = searchCity,
                PagedRoutes = await GetPagedRoutes(searchRoute, searchCity, page)
            };
            return res;
        }

        public async Task<Route> GetRoute(int id)
        {
            return await _context.Routes.AsNoTracking()
                .Include(r => r.Stations).ThenInclude(rs => rs.Station)
                .Include(r => r.Trains).ThenInclude(t => t.Category)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public SelectList GetStationsSelectList(Station selectedStation = null)
        {
            return new SelectList(_context.Stations.AsNoTracking()
                .Select(e => new { e.Id, Name = e.Name + (string.IsNullOrEmpty(e.City) ? "" : $" ({e.City})") }),
                    "Id", "Name", selectedStation);
        }

        public SelectList GetStationsSelectList(IQueryable<Station> stations, Station selectedStation = null)
        {
            return new SelectList(stations
                .Select(e => new { e.Id, Name = e.Name + (string.IsNullOrEmpty(e.City) ? "" : $" ({e.City})") }),
                    "Id", "Name", selectedStation);
        }

        public async Task<string> AddNewRoute(Route route, int stationId)
        {
            try
            {
                RouteStation routeStation = new()
                {
                    RouteId = route.Id,
                    StationId = stationId,
                    StationOrder = 1
                };
                route.Stations.Add(routeStation);
                _context.Add(route);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                if (exc.InnerException is SqlException innerExc)
                {
                    if (innerExc.Number == 2627)
                        return $"Station woth order number 1 already exists in this route.";
                }

                return "Error writing to database.";
            }
            return null;
        }


        private async Task<RouteStation> GetRouteStation(int id)
        {
            return await _context.RouteStations.AsNoTracking()
                                    .Include(r => r.Station)
                                    .FirstOrDefaultAsync(r => r.Id == id);
        }


        private async Task<RouteStation> GetRouteStation(int routeId, int stationId)
        {
            return await _context.RouteStations.AsNoTracking()
                                    .Include(r => r.Station)
                                    .FirstOrDefaultAsync(r => r.RouteId == routeId && r.StationId == stationId);
        }
        private async Task<List<Station>> GetCommStationsId(int prevStationId, int nextStationId, List<int> exceptStationsId)
        {
            if (prevStationId <= 0 && nextStationId <= 0)
                return _context.Stations.AsNoTracking().ToList();

            if (prevStationId == 0)
            {
                prevStationId = nextStationId;
                nextStationId = 0;
            }

            var predicate1 = PredicateBuilder.New<StationRoad>(true);
            predicate1.And(road => road.FirstStationId == prevStationId || road.SecondStationId == prevStationId);

            List<int> result = await _context.StationRoads.AsNoTracking()
                                    .Where(predicate1)
                                    .Select(e => e.FirstStationId == prevStationId ? e.SecondStationId : e.FirstStationId)
                                    .ToListAsync();

            if (nextStationId != 0)
            {
                var predicate2 = PredicateBuilder.New<StationRoad>(false);
                predicate2.Or(road => road.FirstStationId == nextStationId || road.SecondStationId == nextStationId);

                result = (await _context.StationRoads.AsNoTracking()
                .Where(predicate2)
                .Select(e => e.FirstStationId == nextStationId ? e.SecondStationId : e.FirstStationId)
                .ToListAsync())
                .Union(result)
                .ToList();
            }

            exceptStationsId.ForEach(e => result.Remove(e));

            return await _context.Stations.AsNoTracking().Where(e => result.Contains(e.Id)).ToListAsync();
        }


        public async Task<SelectList> GetAllowedStations(RouteStation routeStation)
        {
            if (routeStation == null || routeStation.RouteId == 0)
                return null;

            Route route = await GetRoute(routeStation.RouteId);
            if (route == null)
                return null;

            Dictionary<int, int> stationsByOrder = route.Stations.ToDictionary(key => key.StationOrder, val => val.StationId);

            int prevStation = stationsByOrder.GetValueOrDefault(routeStation.StationOrder - 1);
            int nextStation = stationsByOrder.GetValueOrDefault(routeStation.StationOrder + 1);

            stationsByOrder.Remove(routeStation.StationOrder);

            return GetStationsSelectList(
                (await GetCommStationsId(prevStation, nextStation, stationsByOrder.Values.ToList())).AsQueryable(),
                routeStation.Station);


        }

        public async Task<RoutesStationViewModel> GetRoutesStationViewModel(int routesStationId)
        {
            RouteStation routeStation = await GetRouteStation(routesStationId);
            if (routeStation == null)
                return null;

            return new()
            {
                RouteStation = routeStation,
                StationsSelectList = await GetAllowedStations(routeStation)
            };

        }

        public async Task<string> UpdateRouteStation(RoutesStationViewModel routesEditStation)
        {
            try
            {
                _context.Update(routesEditStation.RouteStation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                if (exc.InnerException is SqlException innerExc)
                {
                    if (innerExc.Number == 2627)
                        return $"Station with id \"{routesEditStation.RouteStation.StationId}\" " +
                            $"or order {routesEditStation.RouteStation.StationOrder}" +
                            $"already exists in this route.";
                }

                return "Error writing to database.";
            }
            return null;
        }

        public async Task<bool> RouteExists(int id)
        {
            return await _context.Routes.AsNoTracking().AnyAsync(r => r.Id == id);
        }

        public int GetNextOrder(Route route)
        {
            int stationOrder = route.Stations.Count + 1;

            if (route.Stations.Any(e => e.StationOrder == stationOrder))
            {
                var orderNum = route.Stations.Select(sr => sr.StationOrder).ToList();
                stationOrder = Enumerable.Range(1, orderNum.Max()).Except(orderNum).ToList().First();
            }
            return stationOrder;
        }

        public async Task<RoutesStationViewModel> GetNewRoutesStationViewModel(int routeId)
        {
            Route route = await GetRoute(routeId);
            if (route == null)
            {
                return null;
            }

            RouteStation rs = new()
            {
                RouteId = routeId,
                StationOrder = GetNextOrder(route)
            };

            return new()
            {
                RouteStation = rs,
                StationsSelectList = await GetAllowedStations(rs)
            };
        }

        public async Task<string> AddRouteStation(RoutesStationViewModel rs)
        {
            try
            {
                await _context.RouteStations.AddAsync(rs.RouteStation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                if (exc.InnerException is SqlException innerExc)
                {
                    if (innerExc.Number == 2627)
                        return "This station already exists in route.";
                }

                return "Error writing to database.";
            }
            return null;
        }

        public async Task RemoveStation(int routeId, int stationId)
        {
            _context.RouteStations.Remove(await GetRouteStation(routeId, stationId));
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTrain(int routeId, int trainId)
        {
            Route route = await _context.Routes.Include(r=> r.Trains).FirstOrDefaultAsync(r=> r.Id == routeId);
            Train train = route.Trains.FirstOrDefault(t => t.Id == trainId);

            route.Trains.Remove(train);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRoute(int routeId)
        {
            Route route = await _context.Routes.FirstOrDefaultAsync(r => r.Id == routeId);
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
        }
    }

}

