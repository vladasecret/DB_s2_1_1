using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.ViewModel.Trains
{
    public class TrainsEditBrigade
    {
        public Train Train { get; set; }
        public PagedResult<Employee> Empls { get; set; }

        public int[] SelectedEmpls { get; set;}

        public int GetTrainId()
        {
            return (int)Train?.Id;
        }

        public int GetPage()
        {
            return Empls?.CurrentPage ?? 1;
        }
    }
}
