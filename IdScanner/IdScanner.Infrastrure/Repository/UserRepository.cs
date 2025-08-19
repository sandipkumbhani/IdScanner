using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using IdScanner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailId == email && u.IsActive);
        }
        //public async Task<ModelUserRole?> GetUserWithRoleAsync(int userRoleId)
        //{
        //    return await _context.modelUserRoles
        //        .FirstOrDefaultAsync(r => r.UserRoleId == userRoleId);
        //}
    }
}
