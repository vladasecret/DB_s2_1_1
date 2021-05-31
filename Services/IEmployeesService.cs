using DB_s2_1_1.EntityModels;
using DB_s2_1_1.ViewModel.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public interface IEmployeesService
    {
        Task<List<Employee>> GetEmployeesIndex();

        Task<Employee> GetEmployee(int id);
        Task<Employee> GetEmployeeDetails(int id);

        Task<EmployeeModViewModel> GetEmployeeMod(int id);

        EmployeeModViewModel GetEmployeeMod();

        Task<string> AddEmployee(Employee employee);

        Task<string> UpdateEmployee(Employee employee);

        Task DeleteEmployee(int id);
    }
}
