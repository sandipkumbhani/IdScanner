using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;

namespace SocPass.Infrastructure.Repository
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
            _context.Roles.Add(userRole);
            await _context.SaveChangesAsync();
            return userRole;
        }
        public async Task<List<UserRole>> GetAllUsersRole()
        {
            return await _context.Roles.Where(u => u.IsActive).ToListAsync();
        }
        public async Task<UserRole?> GetUserRoleById(int roleid)
        {
            return _context.Roles.FirstOrDefault(e => e.UserRoleId == roleid);
        }
        public async Task DeleteRoleAsync(int roleid)
        {
            var existingRole = await _context.Roles.FindAsync(roleid);
            if (existingRole != null)
            {
                existingRole.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
        public async Task UserRoleUpdateAsync(UserRole userRole)
        {
            _context.Roles.Update(userRole);
            _context.SaveChanges();
        }
    }
}
