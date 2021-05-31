using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public interface IUserService 
    {
        Task<bool> Login(string userName, string password);
    }
}
