using DB_s2_1_1.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.ViewModel.Trains
{
    public class TrainEditing
    {
       
        public SelectList Categories { get; set; }
        public SelectList Stations { get; set; }
        public SelectList Routes { get; set; }
        public Train Train { get; set; }
    }
}
