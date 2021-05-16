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
    public class TrainsController : Controller
    {
        private readonly TrainsContext _context;

        public TrainsController(TrainsContext context)
        {
            _context = context;
        }

        // GET: Trains
        public async Task<IActionResult> Index()
        {
            var trainsContext = _context.Trains.Include(t => t.Category).Include(t => t.Station);
            return View(await trainsContext.ToListAsync());
        }

        // GET: Trains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .Include(t => t.Category)
                .Include(t => t.Station)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // GET: Trains/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name");
            ViewData["RouteId"] = new SelectList(_context.Routes.Select(e=> new {RouteId = e.RouteId}).Distinct(), "RouteId", "RouteId");
            return View();
        }

        // POST: Trains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RouteId,StationId,CategoryId,SeatsQty")] Train train)
        {
            if (ModelState.IsValid)
            {
                _context.Add(train);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", train.CategoryId);
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", train.StationId);
            ViewData["RouteId"] = new SelectList(_context.Routes.Select(e => new { RouteId = e.RouteId }).Distinct(), "RouteId", "RouteId");
            return View(train);
        }

        // GET: Trains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains.FindAsync(id);
            if (train == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", train.CategoryId);
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", train.StationId);
            ViewData["RouteId"] = new SelectList(_context.Routes.Select(e => new { RouteId = e.RouteId }).Distinct(), "RouteId", "RouteId");
            return View(train);
        }

        // POST: Trains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RouteId,StationId,CategoryId,SeatsQty")] Train train)
        {
            if (id != train.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(train);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainExists(train.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", train.CategoryId);
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", train.StationId);
            ViewData["RouteId"] = new SelectList(_context.Routes.Select(e => new { RouteId = e.RouteId }).Distinct(), "RouteId", "RouteId");
            return View(train);
        }

        // GET: Trains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .Include(t => t.Category)
                .Include(t => t.Station)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // POST: Trains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            _context.Trains.Remove(train);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainExists(int id)
        {
            return _context.Trains.Any(e => e.Id == id);
        }
    }
}
