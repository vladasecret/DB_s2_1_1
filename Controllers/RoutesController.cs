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
using DB_s2_1_1.Services;
using DB_s2_1_1.ViewModel.Routes;

namespace DB_s2_1_1.Controllers
{
    public class RoutesController : Controller
    {
        private readonly IRoutesService routesService;

        public RoutesController(IRoutesService routesService)
        {
            this.routesService = routesService;
        }

        // GET: Routes
        public async Task<IActionResult> Index(int searchRoute, string searchCity, int page = 1)
        {
            return View(await routesService.GetRoutesIndex(searchRoute, searchCity, page));
        }

        // GET: Routes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var route = await routesService.GetRoute(id);

            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // GET: Routes/Create
        public IActionResult Create()
        {
            ViewBag.StationId = routesService.GetStationsSelectList();
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
                await routesService.AddNewRoute(route, stationId);
                return RedirectToAction(nameof(Index));
            }
            return View(route);
        }

        public async Task<IActionResult> EditStation(int rsId)
        {

            var res = await routesService.GetRoutesStationViewModel(rsId);

            if (res == null)
                return NotFound();

            return View(res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStation(RoutesStationViewModel rs)
        {
            if (ModelState.IsValid)
            {
                string error = await routesService.UpdateRouteStation(rs);

                if (string.IsNullOrWhiteSpace(error))
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", error);
            }
            return View(rs);
        }
        public async Task<IActionResult> AddStation(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var rs = await routesService.GetNewRoutesStationViewModel(id);

            return View(rs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStation(RoutesStationViewModel rs)
        {
            if (ModelState.IsValid)
            {
                string error = await routesService.AddRouteStation(rs);
                if (string.IsNullOrWhiteSpace(error))
                    return RedirectToAction(nameof(Details), new { rs.RouteStation.RouteId });
                ModelState.AddModelError("", error);
            }
            
            return View(routesService.GetNewRoutesStationViewModel(rs.RouteStation.RouteId));
        }

        public async Task<IActionResult> DeleteStation(int routeId, int stationId)
        {
            if (routeId <= 0 || stationId <= 0)
                return NotFound();
            await routesService.RemoveStation(routeId, stationId);
            return RedirectToAction(nameof(Details), new { id = routeId });
        }


        public async Task<IActionResult> DeleteTrain(int routeId, int trainId)
        {
            if (routeId == 0 || trainId == 0)
            {
                return NotFound();
            }
            await routesService.RemoveTrain(routeId, trainId);
            
            return RedirectToAction(nameof(Details), new { id = routeId });
        }

        // GET: Routes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var route = await routesService.GetRoute(id);
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
            await routesService.RemoveRoute(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
