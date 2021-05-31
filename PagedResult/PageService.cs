using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.PagedResult
{
    public static class PageService
    {
        public async static Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query,
                                         int page, int pageSize = 100) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results =await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }

        public static PagedQuery GetPaged(string query, SqlConnection sqlConnection,
                                 int page, int pageSize = 3) 
        {
            var skip = (page - 1) * pageSize;

            DataTable dt = new();

            using SqlDataAdapter adapter = new(query, sqlConnection);
            adapter.Fill(skip, pageSize, dt);
            var result = new PagedQuery
            {
                Results = dt,
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = dt.Rows.Count,
                
            };
            return result;
        }
    }
}
