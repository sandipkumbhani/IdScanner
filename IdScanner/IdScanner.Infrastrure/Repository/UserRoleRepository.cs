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
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _context;
        public UserRoleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UserRole> AddUserRoleAsync(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return userRole;
        }
        public async Task<List<UserRole>> GetAllUsersRole()
        {
            return await _context.UserRoles.Where(u => u.IsActive).ToListAsync();
        }
        public async Task<UserRole> GetUserRoleById(int roleid)
        {
            return _context.UserRoles
                .FirstOrDefault(e => e.UserRoleId == roleid);
        }
        public async Task DeleteRoleAsync(UserRole userRole)
        {
            var existingMenu = await _context.UserRoles.FindAsync(userRole.UserRoleId);
            if (existingMenu != null)
            {
                existingMenu.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
        public async Task UserRoleUpdateAsync(UserRole userRole)
        {
            _context.UserRoles.Update(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
