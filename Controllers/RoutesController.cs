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
    public class RoutesController : Controller
    {
        private readonly TrainsContext _context;

        public RoutesController(TrainsContext context)
        {
            _context = context;
        }

        // GET: Routes
        public async Task<IActionResult> Index(int routeIdFilter, int page = 1)
        {
            ViewData["RouteIdFilter"] = routeIdFilter == 0 ? null : routeIdFilter;
            var trainsContext = routeIdFilter == 0 ?
                _context.RouteStations.Include(r => r.Station)
                : _context.RouteStations
                .Where(e => e.RouteId == routeIdFilter)
                .Include(r => r.Station);
            return View(await trainsContext.GetPaged(page));
        }

        // GET: Routes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.RouteStations
                .Include(r => r.Station)
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
            ViewData["StationId"] = getStationsInfo();
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RouteId,StationId,StationOrder")] RouteStation route)
        {
            if (StationOrderInRoute(route.RouteId, route.StationOrder))
            {
                ModelState.AddModelError("", $"Station with order number {route.StationOrder} already exists in rout with id {route.RouteId}.");
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(route);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", $"The row with values (Route id: {route.RouteId}, Station id: {route.StationId}) already exists.");
                }
            }
            ViewData["StationId"] = getStationsInfo();
            return View(route);
        }

        // GET: Routes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.RouteStations.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            ViewData["StationId"] = getStationsInfo();
            return View(route);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RouteId,StationId,StationOrder")] RouteStation route)
        {
            if (id != route.Id)
            {
                return NotFound();
            }
            if (StationOrderInRoute(route.RouteId, route.StationOrder))
            {
                ModelState.AddModelError("", $"Station with order number {route.StationOrder} already exists in rout with id {route.RouteId}.");
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        _context.Update(route);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RouteExists(route.Id))
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
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", $"The row with values (Route id: {route.RouteId}, Station id: {route.StationId}) already exists.");
                }
            }
            ViewData["StationId"] = getStationsInfo();
            return View(route);
        }

        // GET: Routes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.RouteStations
                .Include(r => r.Station)
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
            var route = await _context.RouteStations.FindAsync(id);
            _context.RouteStations.Remove(route);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private SelectList getStationsInfo()
        {
            return new SelectList(_context.Stations.Select(e => new { Id = e.Id, Name = $"{e.Name}" + ((e.City.Length > 0) ? $" ({e.City})" : "") }), "Id", "Name");
        }

        private bool RouteExists(int id)
        {
            return _context.RouteStations.Any(e => e.Id == id);
        }

        private bool StationOrderInRoute(int routeId, int order)
        {
            return _context.RouteStations.Any(e => e.RouteId == routeId && e.StationOrder == order);
        }
    }
}
