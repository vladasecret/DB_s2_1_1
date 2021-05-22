using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class RouteStation
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Route id")]
        public int RouteId { get; set; }

        [Display(Name = "Station id")]
        public int StationId { get; set; }
      
        [Display(Name = "Station order")]
        public int StationOrder { get; set; }

        public Station Station { get; set; }

        public Route Route { get; set; }
       

    }
}
