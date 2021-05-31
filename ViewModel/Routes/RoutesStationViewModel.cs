using DB_s2_1_1.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.ViewModel.Routes
{
    public class RoutesStationViewModel
    {
        public RouteStation  RouteStation { get; set; }

        public SelectList StationsSelectList { get; set; }
    }
}
