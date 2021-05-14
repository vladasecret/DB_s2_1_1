using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class TransitRoute
    {
        
        public int Id { get; set; }
        public int TransitRouteId { get; set; }
        public int StationOrder { get; set; }
        //public Route Route { get; set; }

        


    }
}
