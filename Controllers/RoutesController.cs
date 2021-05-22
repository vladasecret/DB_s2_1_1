using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using LinqKit;

namespace DB_s2_1_1.Controllers
{
    public class RoutesController : Controller
    {
        private readonly TrainsContext _context;

        public RoutesController(TrainsContext context)
        {
            _context = context;
        }

        // GET: Routes
        public async Task<IActionResult> Index(int searchRoute, string searchCity, int page = 1)
        {
            ViewData["RouteIdFilter"] = searchRoute == 0 ? null : searchRoute;
            ViewData["CityFilter"] = searchCity;
            var predicate = PredicateBuilder.New<Route>(true);
            if (searchRoute > 0)
            {
                predicate.And(item => item.Id == searchRoute);
            }
            if (!string.IsNullOrEmpty(searchCity))
            {
                predicate.And(item => item.Stations.Where(sr => sr.Station.City.ToLower().Contains(searchCity.ToLower())).Any());
            }
            return View(await _context.Routes
                .AsNoTracking()
                .Include(r => r.Stations)
                .ThenInclude(sr => sr.Station)
                .Where(predicate)
                .GetPaged(page));
        }

        // GET: Routes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .AsNoTracking()
                .Include(r => r.Stations)
                .ThenInclude(st => st.Station)
                .Include(r => r.Trains)
                .ThenInclude(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (route == null)
            {
                return NotFound();
            }


            return View(route);
        }

        // GET: Routes/Create
        public IActionResult Create()
        {
            ViewBag.StationId = GetStationsInfo(_context.Stations.AsNoTracking());
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Route route, int stationId)
        {
            if (ModelState.IsValid)
            {
                RouteStation routeStation = new();
                routeStation.Route = route;
                routeStation.Station = await _context.Stations.FirstOrDefaultAsync(e => e.Id == stationId);
                routeStation.StationOrder = 1;
                route.Stations.Add(routeStation);
                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(route);
        }

        public async Task<IActionResult> EditStation(int? rsId)
        {
            if (rsId == null)
            {
                return NotFound();
            }
            RouteStation routeStation = await _context.RouteStations.AsNoTracking()
                .Where(r => r.Id == rsId)
                .Include(r => r.Station)
                .FirstOrDefaultAsync();
            if (routeStation == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.Include(r => r.Stations).FirstOrDefaultAsync(r => r.Id == routeStation.RouteId);
            if (route == null)
            {
                return NotFound();
            }
            Dictionary<int, int> stationsOrder = route.Stations.ToDictionary(key => key.StationOrder, val => val.StationId);

            int prevStation = stationsOrder.GetValueOrDefault(routeStation.StationOrder - 1);
            int nextStation = stationsOrder.GetValueOrDefault(routeStation.StationOrder + 1);
            stationsOrder.Remove(routeStation.StationOrder);
            ViewBag.StationId = GetStationsInfo(FindCommonStations(prevStation, nextStation, stationsOrder.Values.ToList())
                .AsQueryable(), routeStation.Station);


            return View(routeStation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStation(int id, RouteStation rs)
        {
            if (id != rs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(rs.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException exc)
                {
                    ModelState.AddModelError("", exc.InnerException == null ? exc.Message : exc.InnerException.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rs);
        }
        public async Task<IActionResult> AddStation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(e => e.Stations)
                .ThenInclude(stations => stations.Station)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (route == null)
            {
                return NotFound();
            }
            int stationOrder = route.Stations.Count + 1;
            Dictionary<int, int> stationsOrder = route.Stations.ToDictionary(key => key.StationOrder, val => val.StationId);
            if (stationsOrder.Keys.Contains(stationOrder))
            {
                stationOrder = Enumerable.Range(1, stationsOrder.Keys.Max()).Except(stationsOrder.Keys).ToList().First();
            }
            int prevStation = stationsOrder.GetValueOrDefault(stationOrder - 1);
            int nextStation = stationsOrder.GetValueOrDefault(stationOrder + 1);

            ViewBag.StationId = GetStationsInfo(FindCommonStations(prevStation, nextStation, stationsOrder.Values.ToList()).AsQueryable());


            ViewBag.StationOrder = stationOrder;

            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStation(int id, int stationId, int stationOrder)
        {
            Route route = await _context.Routes.Include(r => r.Stations).FirstOrDefaultAsync(r => r.Id == id);
            if (route == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    Station station = await _context.Stations.FirstOrDefaultAsync(s => s.Id == stationId);
                    RouteStation rs = new()
                    {
                        Station = station,
                        StationOrder = stationOrder
                    };
                    route.Stations.Add(rs);
                    _context.Update(route);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException exc)
                {
                    ModelState.AddModelError("", exc.InnerException == null ? exc.Message : exc.InnerException.Message);
                }

            }
            Dictionary<int, int> stationsOrder = route.Stations.ToDictionary(key => key.StationOrder, val => val.StationId);
            int prevStation = stationsOrder.GetValueOrDefault(stationOrder - 1);
            int nextStation = stationsOrder.GetValueOrDefault(stationOrder + 1);
            ViewBag.StationId = GetStationsInfo(FindCommonStations(prevStation, nextStation, stationsOrder.Values.ToList())
                .AsQueryable());

            ViewBag.StationOrder = stationOrder;
            return View(route);
        }

        public async Task<IActionResult> DeleteStation(int routeId, int stationId)
        {
            Route route = await _context.Routes.Include(r => r.Stations)
                .ThenInclude(rs => rs.Station)
                .FirstOrDefaultAsync(r => r.Id == routeId);
            if (route == null)
            {
                return NotFound();
            }
            RouteStation rs = _context.RouteStations.FirstOrDefault(rs => rs.RouteId == routeId && rs.StationId == stationId);
            route.Stations.Remove(rs);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = routeId });
        }


        public async Task<IActionResult> DeleteTrain(int? routeId, int? trainId)
        {
            if (routeId == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.Trains)
                .FirstOrDefaultAsync(m => m.Id == routeId);
            if (route == null)
            {
                return NotFound();
            }
            Train train = route.Trains.Where(t => t.Id == trainId).FirstOrDefault();
            route.Trains.Remove(train);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = routeId });
        }

        // GET: Routes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.Id == id);
        }

        private List<Station> FindCommonStations(int prevStationId, int nextStationId, List<int> exceptValues)
        {
            if (prevStationId + nextStationId == 0)
                return _context.Stations.AsNoTracking().ToList();

            if (prevStationId == 0)
            {
                prevStationId = nextStationId;
                nextStationId = 0;
            }

            var predicate1 = PredicateBuilder.New<StationRoad>(true);
            predicate1.And(road => road.FirstStationId == prevStationId || road.SecondStationId == prevStationId);

            List<int> result =
            _context.StationRoads.AsNoTracking()
                .Where(predicate1)
                .Select(e => e.FirstStationId == prevStationId ? e.SecondStationId : e.FirstStationId)
                .ToList();

            if (nextStationId != 0)
            {
                var predicate2 = PredicateBuilder.New<StationRoad>(false);
                predicate2.Or(road => road.FirstStationId == nextStationId || road.SecondStationId == nextStationId);

                result = _context.StationRoads.AsNoTracking()
                .Where(predicate2)
                .Select(e => e.FirstStationId == nextStationId ? e.SecondStationId : e.FirstStationId)
                .ToList()
                .Union(result)
                .ToList();
            }
            exceptValues.ForEach(e => result.Remove(e));

            return _context.Stations.AsNoTracking().Where(e => result.Contains(e.Id)).ToList();

        }

        private SelectList GetStationsInfo(IQueryable<Station> stations, Station selectedStation = null)
        {

            return new SelectList(stations
                .Select(e => new { e.Id, Name = e.Name + (string.IsNullOrEmpty(e.City) ? "" : $" ({e.City})") }), "Id", "Name", selectedStation);
        }
    }
}
