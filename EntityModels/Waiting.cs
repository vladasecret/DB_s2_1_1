using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class Waiting
    {
        public int Id { get; set; }

        [Display(Name = "Timetable id")]
        public int TimetableId { get; set; }

        public Timetable Timetable { get; set; }

        [Display(Name = "Delay in minutes")]
        public int DelayMinutes { get; set;}
    }
}
