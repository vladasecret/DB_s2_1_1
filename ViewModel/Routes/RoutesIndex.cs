using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.ViewModel.Routes
{
    public class RoutesIndex
    {
        public PagedResult<Route> PagedRoutes { get; set; }

        public int? SearchRoute { get; set; }
        public string SearchCity { get; set; }
    }
}
