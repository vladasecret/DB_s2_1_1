using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.PagedResult
{
    public class PagedQuery : PagedResultBase
    {
        public DataTable Results { get; set; }

        public PagedQuery()
        {
            Results = new();
        }
        
    }
}
