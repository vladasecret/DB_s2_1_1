using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class Train
    {
        [Display(Name = "Train id")]
        public int Id { get; set; }

        [Display(Name = "Route id")]
        public int? RouteId { get; set; }

        public int? StationId { get; set; }

        [Display(Name = "Category Id")]
        public int? CategoryId { get; set; }

        [Display(Name = "Quantity of seats")]
        public int SeatsQty { get; set; }

        public Category Category { get; set; }

        public Station Station { get; set; }
        
        public Route Route { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
