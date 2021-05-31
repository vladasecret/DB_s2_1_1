using DB_s2_1_1.PagedResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.ViewModel
{
    public class ManualQueryViewModel
    {
        public PagedQuery PagedQuery { get; set; }
        public string InsertedQuery { get; set; }
        public int Page { get; set; }

        public string ErrorMsg { get; set; }
    }
}
