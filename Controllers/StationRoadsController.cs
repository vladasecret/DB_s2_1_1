using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DB_s2_1_1.EntityModels;

namespace DB_s2_1_1.Controllers
{
    public class StationRoadsController : Controller
    {
        private readonly TrainsContext _context;

        public StationRoadsController(TrainsContext context)
        {
            _context = context;
        }

        // GET: StationRoads
        public async Task<IActionResult> Index()
        {
            var trainsContext = _context.StationRoads.Include(s => s.FirstStation).Include(s => s.SecondStation);
            return View(await trainsContext.ToListAsync());
        }

        // GET: StationRoads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationRoad = await _context.StationRoads
                .Include(s => s.FirstStation)
                .Include(s => s.SecondStation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stationRoad == null)
            {
                return NotFound();
            }

            return View(stationRoad);
        }

        // GET: StationRoads/Create
        public IActionResult Create()
        {
            ViewData["FirstStationId"] = new SelectList(_context.Stations, "Id", "Name");
            ViewData["SecondStationId"] = new SelectList(_context.Stations, "Id", "Name");
            return View();
        }

        // POST: StationRoads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstStationId,SecondStationId,Distance")] StationRoad stationRoad)
        {
            if (stationRoad.FirstStationId == stationRoad.SecondStationId)
            {
                ModelState.AddModelError("", "First station and second station cannot be the equal");
            }
            else if(StationRoadExists(stationRoad.FirstStationId, stationRoad.SecondStationId)) {
                ModelState.AddModelError("", $"Road between stations {stationRoad.FirstStationId} and {stationRoad.SecondStationId} " +
                    $"already exists.");
            }
            else if (ModelState.IsValid)
            {
                _context.Add(stationRoad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstStationId"] = new SelectList(_context.Stations, "Id", "Name", stationRoad.FirstStationId);
            ViewData["SecondStationId"] = new SelectList(_context.Stations, "Id", "Name", stationRoad.SecondStationId);
            return View(stationRoad);
        }

        // GET: StationRoads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationRoad = await _context.StationRoads.FindAsync(id);
            if (stationRoad == null)
            {
                return NotFound();
            }
            ViewData["FirstStationId"] = new SelectList(_context.Stations, "Id", "Name", stationRoad.FirstStationId);
            ViewData["SecondStationId"] = new SelectList(_context.Stations, "Id", "Name", stationRoad.SecondStationId);
            return View(stationRoad);
        }

        // POST: StationRoads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstStationId,SecondStationId,Distance")] StationRoad stationRoad)
        {
            if (id != stationRoad.Id)
            {
                return NotFound();
            }
            if (stationRoad.FirstStationId == stationRoad.SecondStationId)
            {
                ModelState.AddModelError("", "First station and second station cannot be the equal");
            }
            else if (StationRoadExists(stationRoad.FirstStationId, stationRoad.SecondStationId))
            {
                ModelState.AddModelError("", $"Road between stations {stationRoad.FirstStationId} and {stationRoad.SecondStationId} " +
                    $"already exists.");
            }
            else
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stationRoad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationRoadExists(stationRoad.Id))
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
            ViewData["FirstStationId"] = new SelectList(_context.Stations, "Id", "Name", stationRoad.FirstStationId);
            ViewData["SecondStationId"] = new SelectList(_context.Stations, "Id", "Name", stationRoad.SecondStationId);
            return View(stationRoad);
        }

        // GET: StationRoads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stationRoad = await _context.StationRoads
                .Include(s => s.FirstStation)
                .Include(s => s.SecondStation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stationRoad == null)
            {
                return NotFound();
            }

            return View(stationRoad);
        }

        // POST: StationRoads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stationRoad = await _context.StationRoads.FindAsync(id);
            _context.StationRoads.Remove(stationRoad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StationRoadExists(int id)
        {
            return _context.StationRoads.Any(e => e.Id == id);
        }
        private bool StationRoadExists(int firstId, int secondId) 
        {
            return _context.StationRoads.Any(e=> (e.FirstStationId == firstId && e.SecondStationId == secondId) 
            || (e.FirstStationId == secondId && e.SecondStationId == firstId));
        }
    }
}
