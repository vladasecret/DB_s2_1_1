using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.ViewModel.Timetables
{
    public class TimetablesCreateModel
    {
        public SelectList TrainsSelectList { get; set; }

        public int TrainId { get; set; }

        public DateTime ArrivalTime { get; set; }

        public bool TrainDirection { get; set; }
    }
}
