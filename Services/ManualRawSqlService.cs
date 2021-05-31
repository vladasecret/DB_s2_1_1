using DB_s2_1_1.PagedResult;
using DB_s2_1_1.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public class ManualRawSqlService : IManualRawSqlService
    {
        private readonly IConfiguration configuration;

        public ManualRawSqlService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ManualQueryViewModel ExecuteQuery(string query, int page)
        {
            ManualQueryViewModel result = new()
            {
                InsertedQuery = query,
            };


            if (!string.IsNullOrEmpty(query))
            {
                using SqlConnection sqlConnection = new(configuration.GetConnectionString("ManualRawSqlConnection"));
                sqlConnection.Open();
                using SqlDataAdapter adapter = new(query, sqlConnection);
                try
                {
                    page = Math.Max(page, 1);

                    result.PagedQuery = PageService.GetPaged(query, sqlConnection, page);
                }
                catch (SqlException exc)
                {
                    result.ErrorMsg = exc.Message;
                }
            }

            return result;
        }
    }
}
