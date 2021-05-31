using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using DB_s2_1_1.ViewModel.Trains;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.ViewModel.Trains
{
    public class TrainsIndex
    {
        public PagedResult<TrainsViewModel> PagedTrains { get; set; }
        public TrainsFilters TrainsFilters{ get; set; }
        public List<int> RouteIdList { get; set; }
        public SelectList Categories { get; set; }

    }
}
