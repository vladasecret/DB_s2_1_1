using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class StationRoad
    {
        public int Id { get; set; }
        public int FirstStationId { get; set; }
        public int SecondStationId { get; set; }
        public double Distance { get; set; }
        public Station FirstStation { get; set; }
        public Station SecondStation { get; set; }
    }
}
