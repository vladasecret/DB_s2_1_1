using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class Route
    {

        public int Id { get; set; }

        public ICollection<RouteStation> Stations { get; set; } = new List<RouteStation>();

        public ICollection<Train> Trains { get; set; } = new List<Train>();
    }
}
