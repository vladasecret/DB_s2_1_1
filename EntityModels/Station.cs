using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class Station
    {
        [Display(Name = "Station id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Station name")]
        public string Name { get; set; }

        [Display(Name = "Station city")]
        public string? City { get; set; }
        public ICollection<Train> Trains { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<RouteStation> Routes { get; set; }
        public ICollection<Timetable> Timetables { get; set; }


    }
}
