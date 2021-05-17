using System;
using System.Collections.Generic;
using System.Linq;
using LinqKit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DB_s2_1_1.EntityModels;
using DB_s2_1_1.ViewModel;
using DB_s2_1_1.PagedResult;


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
        public async Task<IActionResult> Index(int searchRoute, int searchCategory, int searchSeats, int page = 1)
        {
            ViewData["CategoryFilter"] = searchCategory;
            ViewData["RouteIdFilter"] = searchRoute > 0 ? searchRoute : null;
            ViewData["SeatsFilter"] = searchSeats > 0 ? searchSeats : null;

            ViewData["RouteId"] = await _context.Routes.Select(e => e.RouteId).Distinct().ToListAsync();
            ViewData["Categories"] = new SelectList(_context.Categories.AsNoTracking(), "Id", "Name", searchCategory);

            var predicate = PredicateBuilder.New<Train>(true);

            if (searchCategory != 0)
                predicate.And(e => e.CategoryId == searchCategory);

            if (searchSeats > 0)
                predicate.And(e => e.SeatsQty >= searchSeats);
            if (searchRoute > 0)
                predicate.And(e => e.RouteId == searchRoute);

            var trainsContext = _context.Trains
                .AsNoTracking()
                .Where(predicate)
                .Select(e => new TrainsViewModel
                {
                    Id = e.Id,
                    Category = e.Category.Name,
                    SeatsQty = e.SeatsQty,
                    Station = e.Station.Name,
                    RouteId = e.RouteId
                });

            return View(await trainsContext.GetPaged(page));
        }

        // GET: Trains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .AsNoTracking()
                .Select(e => new TrainsViewModel
                {
                    Id = e.Id,
                    Category = e.Category.Name,
                    SeatsQty = e.SeatsQty,
                    Station = e.Station.Name,
                    RouteId = e.RouteId,
                    Employees = e.Employees
                })
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
            ViewData["RouteId"] = new SelectList(_context.Routes.Select(e => new { RouteId = e.RouteId }).Distinct(), "RouteId", "RouteId");
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

        // GET: Trains/Edit/5
        public async Task<IActionResult> EditBrigade(int? id)
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
            ViewData["SelectedEmpls"] = train.Employees;            
            
            return View(await _context.Employees.AsNoTracking().ToListAsync());
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
