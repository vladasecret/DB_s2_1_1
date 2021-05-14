using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class Employee
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public int StationId { get; set; }

        public ICollection<Train> Trains { get; set; }
    }
}
