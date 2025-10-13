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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> AddUserAsync(User user)
        {
            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.users.AnyAsync(u => u.EmailId == email);
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.users.Include(x => x.UserRole).Include(x=>x.Society).Where(u => u.IsActive).OrderBy(u => u.Name).ToListAsync();
        }
        public async Task<User?> GetUserById(int id)
        {
            var user = await _context.users
                .FirstOrDefaultAsync(e => e.UserId == id && e.IsActive==true);
            return user;
        }
        public async Task DeleteAsync(User user)
        {
            var existingMenu = await _context.users.FindAsync(user.UserId);
            if (existingMenu != null)
            {
                existingMenu.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
        public async Task UserUpdateAsync(User user)
        {
            _context.users.Update(user);
            _context.SaveChanges();
        }


    }
}
