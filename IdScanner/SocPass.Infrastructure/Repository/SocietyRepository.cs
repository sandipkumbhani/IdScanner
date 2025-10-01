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
    public class SocietyRepository : ISocietyRepository
    {
        private AppDbContext _context;
        public SocietyRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<Society> CreateSocietyAsync(Society society)
        {
            _context.societies.Add(society);
            await _context.SaveChangesAsync();
            return society;
        }
        public async Task<Society> GetByIdAsync(int societyid)
        {
            return await _context.societies.Where(x => x.IsActive == true).FirstOrDefaultAsync(e => e.SocietyId == societyid);
        }
        public async Task<List<Society>> GetAllSocietyAsync(int userId)
        {
            var user = await _context.users
                .Include(u => u.Society)
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return new List<Society>();

            if (user.SocietyId == null &&
                user.UserRole != null &&
                user.UserRole.Name == "Admin")
            {
                return await _context.societies
                    .Where(s => s.IsActive)
                    .Select(s => new Society
                    {
                        SocietyId = s.SocietyId,
                        Name = s.Name,
                        Address = s.Address,
                        Email= s.Email,
                        Contact = s.Contact,
                        Contact2 = s.Contact2,
                        IsActive = s.IsActive
                    })
                    .ToListAsync();
            }

            if (user.Society != null && user.Society.IsActive)
            {
                return new List<Society>
                 {
            new Society
                {
                    SocietyId = user.Society.SocietyId,
                    Name = user.Society.Name,
                    Address=user.Society.Address,
                    Email=user.Society.Email,
                    Contact=user.Society.Contact,
                    Contact2=user.Society.Contact2,
                    IsActive = user.Society.IsActive
                }
            };
            }

            return new List<Society>();
        }
        public async Task UpdateSocietyAsync(Society society)
        {
            _context.societies.Update(society);
            _context.SaveChanges();
        }
        public async Task DeleteSocietyAsync(Society society)
        {
            var existingSociety = await _context.societies.FindAsync(society.SocietyId);
            if (existingSociety != null)
            {
                existingSociety.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

    }
}
