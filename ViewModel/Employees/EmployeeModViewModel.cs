using DB_s2_1_1.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.ViewModel.Employees
{
    public class EmployeeModViewModel
    {
        public Employee Employee { get; set; }
        public SelectList Stations { get; set; }
    }
}
