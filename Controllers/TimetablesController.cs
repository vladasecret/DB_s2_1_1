using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;

namespace DB_s2_1_1.Controllers
{
    public class TimetablesController : Controller
    {
        private readonly TrainsContext _context;

        public TimetablesController(TrainsContext context)
        {
            _context = context;
        }

        // GET: Timetables
        public async Task<IActionResult> Index(int page = 1)
        {
            var trainsContext = _context.Timetables.Include(t => t.Station).Include(t => t.Train);
            return View(await trainsContext.AsNoTracking().GetPaged(page));
        }

        // GET: Timetables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetables
                .Include(t => t.Station)
                .Include(t => t.Train)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timetable == null)
            {
                return NotFound();
            }

            return View(timetable);
        }


        public IActionResult Create()
        {
            ViewData["TrainId"] = new SelectList(_context.Trains.AsNoTracking().Where(e => e.RouteId != null), "Id", "Id");
            return View();
        }

        // POST: Timetables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int trainId, DateTime arrivalTime, bool trainDirection)
        {
            Train train = await _context.Trains.AsNoTracking()
                .Include(t => t.Route)
                .ThenInclude(r => r.Stations)
                .ThenInclude(rs => rs.Station)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == trainId);
            if (train == null)
            {
                return NotFound();
            }

            IQueryable<Timetable> trainTimetables = _context.Timetables.Where(t => t.TrainId == trainId);

            DateTime LastDepartureTime = trainTimetables.Any() ? trainTimetables.Max(t => t.DepartureTime) : DateTime.MinValue;
            int roadId = trainTimetables.Any() ? trainTimetables.Max(t => t.RoadId) + 1 : 1;

            if (LastDepartureTime.CompareTo(arrivalTime) >= 0)
            {
                ModelState.AddModelError("Arrival time error", $"Arrival time must be greater than {LastDepartureTime}");
            }

            else if (ModelState.IsValid)
            {
                List<Station> stationsOrder = trainDirection ? train.Route.Stations.OrderByDescending(e => e.StationOrder).Select(e => e.Station).ToList()
                    : train.Route.Stations.OrderBy(e => e.StationOrder).Select(e => e.Station).ToList();

                double speed = train.Category.Speed;
                if (speed < 0)
                    speed = 30;

                int prevStationId = 0;
                double distance = 0;
                DateTime departureTime = new();
                Random rand = new();

                foreach (Station station in stationsOrder)
                {
                    distance = getDistance(prevStationId, station.Id);
                    arrivalTime = arrivalTime.AddMinutes(distance / speed * 60);
                    departureTime = arrivalTime.AddMinutes(rand.Next(5, 60));
                    prevStationId = station.Id;

                    _context.Timetables.Add(new Timetable
                    {
                        RoadId = roadId,
                        TrainId = train.Id,
                        StationId = station.Id,
                        ArrivalTime = arrivalTime,
                        DepartureTime = departureTime,
                        TrainDirection = trainDirection
                    });

                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoadId"] = roadId;
            ViewData["TrainId"] = new SelectList(_context.Trains, "Id", "Id", trainId);
            return View();
        }

        // GET: Timetables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetables.FindAsync(id);
            if (timetable == null)
            {
                return NotFound();
            }
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", timetable.StationId);
            ViewData["TrainId"] = new SelectList(_context.Trains, "Id", "Id", timetable.TrainId);
            return View(timetable);
        }

        // POST: Timetables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoadId,TrainId,StationId,ArrivalTime,DepartureTime,TrainDirection")] Timetable timetable)
        {
            if (id != timetable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timetable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimetableExists(timetable.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", timetable.StationId);
            ViewData["TrainId"] = new SelectList(_context.Trains, "Id", "Id", timetable.TrainId);
            return View(timetable);
        }

        // GET: Timetables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetables
                .Include(t => t.Station)
                .Include(t => t.Train)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timetable == null)
            {
                return NotFound();
            }

            return View(timetable);
        }

        // POST: Timetables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timetable = await _context.Timetables.FindAsync(id);
            _context.Timetables.Remove(timetable);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimetableExists(int id)
        {
            return _context.Timetables.Any(e => e.Id == id);
        }

        private double getDistance(int firstId, int secondId)
        {
            return _context.StationRoads.AsNoTracking()
                .Where(e => (e.FirstStationId == firstId && e.SecondStationId == secondId)
                || (e.FirstStationId == secondId && e.SecondStationId == firstId))
                .Select(e => e.Distance)
                .FirstOrDefault();
        }
    }
}
