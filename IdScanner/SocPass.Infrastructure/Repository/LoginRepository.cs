using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Infrastructure.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AppDbContext _context;
        public LoginRepository(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<User?> GetByEmailAsync(string email)
        //{
        //    return await _context.users.FirstOrDefaultAsync(u => u.EmailId == email && u.IsActive);
        //}
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.users
                .Include(u => u.Society)
                    .ThenInclude(s => s.Subscriptions)
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.EmailId == email && u.IsActive);
        }

    }
}
