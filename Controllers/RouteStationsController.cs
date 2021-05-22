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
    public class RouteStationsController : Controller
    {
        private readonly TrainsContext _context;

        public RouteStationsController(TrainsContext context)
        {
            _context = context;
        }

        // GET: RouteStations
        public async Task<IActionResult> Index()
        {
            var trainsContext = _context.RouteStations.Include(r => r.Station);
            return View(await trainsContext.ToListAsync());
        }

        // GET: RouteStations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeStation = await _context.RouteStations
                .Include(r => r.Station)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (routeStation == null)
            {
                return NotFound();
            }

            return View(routeStation);
        }

        // GET: RouteStations/Create
        public IActionResult Create()
        {
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name");
            return View();
        }

        // POST: RouteStations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RouteId,StationId,StationOrder")] RouteStation routeStation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(routeStation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", routeStation.StationId);
            return View(routeStation);
        }

        // GET: RouteStations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeStation = await _context.RouteStations.FindAsync(id);
            if (routeStation == null)
            {
                return NotFound();
            }
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", routeStation.StationId);
            return View(routeStation);
        }

        // POST: RouteStations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RouteId,StationId,StationOrder")] RouteStation routeStation)
        {
            if (id != routeStation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(routeStation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteStationExists(routeStation.Id))
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
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", routeStation.StationId);
            return View(routeStation);
        }

        // GET: RouteStations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeStation = await _context.RouteStations
                .Include(r => r.Station)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (routeStation == null)
            {
                return NotFound();
            }

            return View(routeStation);
        }

        // POST: RouteStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routeStation = await _context.RouteStations.FindAsync(id);
            _context.RouteStations.Remove(routeStation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteStationExists(int id)
        {
            return _context.RouteStations.Any(e => e.Id == id);
        }
    }
}
