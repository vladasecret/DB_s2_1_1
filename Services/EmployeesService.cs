using DB_s2_1_1.EntityModels;
using DB_s2_1_1.ViewModel.Employees;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly TrainsContext _context;

        public EmployeesService(TrainsContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetEmployeesIndex()
        {
            return await _context.Employees.AsNoTracking().Include(e => e.Station).ToListAsync();
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await _context.Employees.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Employee> GetEmployeeDetails(int id)
        {
            return await _context.Employees.AsNoTracking()
                .Include(e => e.Station)
                .Include(e => e.Trains)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public SelectList GetStationsSelectList(int? SelectedStation = null)
        {
            return new SelectList(_context.Stations, "Id", "Name", SelectedStation);

        }
        public async Task<EmployeeModViewModel> GetEmployeeMod(int id)
        {
            var empl = await GetEmployee(id);
            if (empl == null)
                return null;
            return new()
            {
                Employee = empl,
                Stations = GetStationsSelectList(empl.StationId)
            };
        }

        public EmployeeModViewModel GetEmployeeMod()
        {
            return new()
            {
                Stations = GetStationsSelectList()
            };
        }

        public async Task<string> AddEmployee(Employee employee)
        {
            try
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return "Database writing exception.";
            }
            return null;
        }
        public async Task<string> UpdateEmployee(Employee employee)
        {
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return "Database writing exception.";
            }
            return null;
        }

        public async Task DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
