using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace DB_s2_1_1.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly TrainsContext _context;

        public CategoriesService(TrainsContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Category>> GetPagedCategories(int page = 1)
        {
            return await _context.Categories.AsNoTracking().GetPaged(page);
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<string> AddCategory(Category category)
        {
            try
            {
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                if (exc.InnerException is SqlException innerExc)
                {
                    if (innerExc.Number == 2627)
                        return $"Category with name \"{category.Name}\" already exists.";
                }
                return "Error writing to database.";
            }
            return null;
        }

        public async Task<string> UpdateCategory(Category category)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                if (exc.InnerException is SqlException innerExc)
                {
                    if (innerExc.Number == 2627)
                        return $"Category with name \"{category.Name}\" already exists.";
                }

                return "Error writing to database.";
            }

            return null;
        }

        public async Task<bool> CategoryExists(int id)
        {
            return await _context.Categories.AsNoTracking().AnyAsync(e => e.Id == id);
        }

        public async Task RemoveCategory(int id)
        {
            Category category = await GetCategory(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
