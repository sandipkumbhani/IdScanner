using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;

namespace SocPass.Infrastructure.Repository
{
    public class    SocietyDataRepository : ISocietyDataRepository
    {
        private readonly AppDbContext _context;
        public SocietyDataRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SocietyData>> GetAllSocietyDataAsync()
        {
            return await _context.SocietyData
        .Include(sd => sd.Flat)
            .ThenInclude(f => f.Block)
                .ThenInclude(b => b.Society)
        .Where(sd => sd.IsActive)
        .ToListAsync();
        }

        public async Task<SocietyData> AddSocietyDataAsync(SocietyData societyData)
        {
            _context.SocietyData.Add(societyData);
            await _context.SaveChangesAsync();
            return societyData;
        }
        public async Task<SocietyData> GetSocietyDataByIdAsync(int societyDataId)
        {
            //return await _context.SocietyData.Include(e=> e.Flat).
            //Where(x => x.IsActive == true).FirstOrDefaultAsync(e => e.SocietyDataId == societyDataId);

            return await _context.SocietyData
       .Include(sd => sd.Flat)                     // Include Flat
           .ThenInclude(f => f.Block)             // Include Block inside Flat
               .ThenInclude(b => b.Society)       // Include Society inside Block
       .Where(sd => sd.IsActive)                  // Only active records
       .FirstOrDefaultAsync(sd => sd.SocietyDataId == societyDataId);
        }

        public async Task UpdateSocietyAsync(SocietyData societyData)
        {
            _context.SocietyData.Update(societyData);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSocietyAsync(int societyDataId)
        {
            var existingData = await _context.SocietyData.FindAsync(societyDataId);
            if (existingData != null)
            {
                existingData.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<SocietyData>> GetSocietyDataByFlatId(int flatId)
        {
            return await _context.SocietyData.Include(e => e.Flat)
                .Where(x => x.IsActive == true && x.FlatId == flatId).ToListAsync();
        }
    }
}
