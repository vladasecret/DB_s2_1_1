using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DB_s2_1_1.EntityModels;
using DB_s2_1_1.Services;

namespace DB_s2_1_1.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        // GET: Categories
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await categoriesService.GetPagedCategories(page));
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
                return NotFound();

            var category = await categoriesService.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Speed")] Category category)
        {

            if (ModelState.IsValid)
            {
                string error = await categoriesService.AddCategory(category);

                if (string.IsNullOrWhiteSpace(error))
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", error);
            }
            return View(category);

        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var category = await categoriesService.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Speed")] Category category)
        {
            if (id != category.Id || !(await categoriesService.CategoryExists(id)))
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                string error = await categoriesService.UpdateCategory(category);

                if (string.IsNullOrWhiteSpace(error))
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", error);
            }

            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var category = await categoriesService.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await categoriesService.RemoveCategory(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
