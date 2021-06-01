using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using DB_s2_1_1.Services;
using DB_s2_1_1.ViewModel.Timetables;

namespace DB_s2_1_1.Controllers
{
    public class TimetablesController : Controller
    {
        private readonly ITimetablesService timetablesService;

        public TimetablesController(ITimetablesService timetablesService)
        {
            this.timetablesService = timetablesService;
        }

        // GET: Timetables
        public async Task<IActionResult> Index(int page = 1)
        {

            return View(await timetablesService.GetTimetablesIndex(page));
        }

        // GET: Timetables/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var timetable = await timetablesService.GetTimetableDetails(id);

            if (timetable == null)
            {
                return NotFound();
            }

            return View(timetable);
        }


        public IActionResult Create()
        {
            return View(timetablesService.GetTimetablesCreateModel());
        }

        // POST: Timetables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimetablesCreateModel createModel)
        {
            if (createModel == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string error = await timetablesService.GenerateTimetables(createModel);
                if (string.IsNullOrWhiteSpace(error))
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", error);
            }
            
            createModel.TrainsSelectList = timetablesService.GetTrainsIdSelectList(createModel.TrainId);
            return View(createModel);
        }
        /*
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

        private double GetDistance(int firstId, int secondId)
        {
            return _context.StationRoads.AsNoTracking()
                .Where(e => (e.FirstStationId == firstId && e.SecondStationId == secondId)
                || (e.FirstStationId == secondId && e.SecondStationId == firstId))
                .Select(e => e.Distance)
                .FirstOrDefault();
        }
        */
    }
}
