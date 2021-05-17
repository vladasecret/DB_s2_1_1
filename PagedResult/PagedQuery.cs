using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.PagedResult
{
    public static class PagedQuery
    {
        public async static Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query,
                                         int page, int pageSize = 100) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results =await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
    }
}
