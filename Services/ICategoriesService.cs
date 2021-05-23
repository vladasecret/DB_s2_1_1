using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public interface ICategoriesService 
    {

        Task<PagedResult<Category>> GetPagedCategories(int page = 1);

        Task<Category> GetCategory(int id);

        Task<string> AddCategory(Category category);

        Task<string> UpdateCategory(Category category);

        Task<bool> CategoryExists(int id);

        Task RemoveCategory(int id);
    }
}
