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

namespace DB_s2_1_1.Controllers
{
    public class StationsController : Controller
    {
        private readonly IStationsService stationsService;

        public StationsController(IStationsService stationsService)
        {
            this.stationsService = stationsService;
        }

        // GET: Stations
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await stationsService.GetPagedStations(page));
        }

        // GET: Stations/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var station = await stationsService.GetStation(id);

            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        // GET: Stations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,City")] Station station)
        {
            if (ModelState.IsValid)
            {
                string error = await stationsService.AddStation(station);
                if (string.IsNullOrWhiteSpace(error))
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", error);
            }
            return View(station);
        }

        // GET: Stations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var station = await stationsService.GetStation(id);

            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,City")] Station station)
        {
            if (id != station.Id || !(await stationsService.StationExists(station.Id)))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string error = await stationsService.UpdateStation(station);

                if (string.IsNullOrWhiteSpace(error))
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", error);
            }
            return View(station);
        }

        // GET: Stations/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var station = await stationsService.GetStation(id);

            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await stationsService.RemoveStation(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
