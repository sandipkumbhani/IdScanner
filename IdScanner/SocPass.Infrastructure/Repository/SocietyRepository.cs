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
        public async Task<Society>GetByIdAsync(int societyid)
        {
            return await _context.societies.Where(x=>x.IsActive==true).FirstOrDefaultAsync(e => e.SocietyId == societyid);
        }
        public async Task<List<Society>>GetAllSocietyAsync()
        {
            return await _context.societies.Where(x =>x.IsActive==true).ToListAsync();
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
