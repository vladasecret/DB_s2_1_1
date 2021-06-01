using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class Timetable
    {
        public int Id { get; set; }
        public int RoadId { get; set; }
        public int TrainId { get; set; }
        public int StationId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public bool TrainDirection { get; set; }

        public Train Train { get; set; }
        public Station Station { get; set; }
        public ICollection<Waiting> Waitings { get; set; }
       
    }
}
