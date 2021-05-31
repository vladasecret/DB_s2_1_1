using DB_s2_1_1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public interface IManualRawSqlService
    {
        ManualQueryViewModel ExecuteQuery(string query, int page);
    }
}
