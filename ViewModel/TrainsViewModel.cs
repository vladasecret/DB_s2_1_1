using DB_s2_1_1.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.ViewModel
{
    public class TrainsViewModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int SeatsQty { get; set; }

        public string Station { get; set; }
        public int? RouteId { get; set; }
        public Route Route { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
