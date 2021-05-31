using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using DB_s2_1_1.Cryptography;

namespace DB_s2_1_1.Services
{
    public class UserService : IUserService
    {
        private readonly TrainsContext _context;

        public UserService(TrainsContext context)
        {
            _context = context;
        }

        public async Task<bool> Login(string userName, string password)
        {
            /*
            OldUser user = await _context.OldUsers.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == userName);

            if (user != null && user.Password.Equals(AES.EncryptText(password)))
                return true;
            */

            return false;
        }
        
    }
}
