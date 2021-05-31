using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DB_s2_1_1.EntityModels;
using DB_s2_1_1.Services;
using DB_s2_1_1.ViewModel.Employees;

namespace DB_s2_1_1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            this.employeesService = employeesService;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await employeesService.GetEmployeesIndex());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var employee = await employeesService.GetEmployeeDetails(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View(employeesService.GetEmployeeMod());
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FIO,StationId")] EmployeeModViewModel employeeMod)
        {
            if (ModelState.IsValid)
            {
                string error = await employeesService.AddEmployee(employeeMod.Employee);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(await employeesService.GetEmployeeMod(employeeMod.Employee.Id));
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var employee = await employeesService.GetEmployeeMod(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Employee,Employee.Id,Employee.FIO,Employee.StationId")] EmployeeModViewModel employeeMod)
        {
            if (id != employeeMod.Employee?.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string error = await employeesService.UpdateEmployee(employeeMod.Employee);
                if (string.IsNullOrWhiteSpace(error))
                    return RedirectToAction(nameof(Index));
            }

            return View(id);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var employee =await employeesService.GetEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await employeesService.DeleteEmployee(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
