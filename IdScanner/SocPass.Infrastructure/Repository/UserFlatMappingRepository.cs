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
    public class UserFlatMappingRepository : IUserFlatMappingRepository
    {
        private readonly AppDbContext _context;
        public UserFlatMappingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UserFlatMapping> AddFlatMappingAsync(UserFlatMapping userFlatMapping)
        {
            _context.userFlatMappings.Add(userFlatMapping);
            await _context.SaveChangesAsync();
            return userFlatMapping;

        }
        public async Task<List<Member>> GetMembersByUserIdAsync(int loginUserId)
        {
            var flatId = await _context.userFlatMappings
                .Where(x => x.UserId == loginUserId && x.IsActive==true)
                .Select(x => x.FlatId)
                .FirstOrDefaultAsync(); 

            if (flatId == 0)
            {
                return new List<Member>();
            }
            var members = await _context.members
                .Where(m => m.FlatId == flatId && m.IsActive)
                .ToListAsync();

            return members;
        }
        public async Task<UserFlatMapping> GetMappingByUserId(int userid)
        {
            return await _context.userFlatMappings
                .FirstOrDefaultAsync(e => e.UserId == userid && e.IsActive==true);
        }

        public async Task UpdateFlatMappingAsync(UserFlatMapping userFlatMapping)
        {
            _context.userFlatMappings.Update(userFlatMapping);
            await _context.SaveChangesAsync();
        }

    }
}
