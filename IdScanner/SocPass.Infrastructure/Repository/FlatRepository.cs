using Microsoft.EntityFrameworkCore;
using SocPass.Domain.DTO;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Infrastructure.Repository
{
    public class FlatRepository : IFlatRepository
    {
        private readonly AppDbContext _context;
        public FlatRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Flat> CreateFlatAsync(Flat flat)
        {
            _context.flats.Add(flat);
            await _context.SaveChangesAsync();
            return flat;
        }
        public async Task<List<Flat>> GetAllFlatAsync()
        {
            return await _context.flats.Include(X => X.Society).Include(X => X.Block).Where(X => X.IsActive == true).ToListAsync();
        }
        public async Task<Flat> GetById(int flatId)
        {
            return await _context.flats
                .Include(x => x.Society)
                .Include(x => x.Block)
                .Where(x => x.IsActive == true)
                .FirstOrDefaultAsync(x => x.FlatId == flatId);
        }
        public async Task<Flat?> GetFlatByPositionAsync(int societyId, int blockId)
        {
            return await _context.flats
                .Where(f => f.SocietyId == societyId && f.BlockId == blockId)
                .OrderBy(f => f.FlatId)
                .FirstOrDefaultAsync();
        }
        public async Task<Flat> UpdateAsync(Flat flat)
        {
            _context.flats.Update(flat);
            await _context.SaveChangesAsync();
            return flat;
        }
        public async Task<List<Flat>> UpdateRangeAsync(List<Flat> flats)
        {
            _context.flats.UpdateRange(flats);
            await _context.SaveChangesAsync();
            return flats;
        }
        public async Task<Flat> GetFlatByNumberAsync(int societyId, int blockId, int flatNumber)
        {
            return await _context.flats
                .FirstOrDefaultAsync(f => f.SocietyId == societyId
                                       && f.BlockId == blockId
                                       && f.FlatNumber == flatNumber.ToString());
        }
        public async Task<Flat> UpdateFlatAsync(Flat flat)
        {
            _context.flats.Update(flat);
            await _context.SaveChangesAsync();
            return flat;
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
        public async Task<List<Flat>> GetFlatsByBlockAsync(int societyId, int blockId)
        {
            return await _context.flats
                .Where(f => f.SocietyId == societyId && f.BlockId == blockId)
                .ToListAsync() ?? new List<Flat>();
        }

        public async Task DeleteFlatAsync(int flatId)
        {
            var member = await _context.members.FindAsync(flatId);
            if (member != null)
            {
                member.IsActive = false;
                await _context.SaveChangesAsync();
            };
        }
        public async Task<List<Flat>> GetFlatByBlockIdAsync(int blockid)
        {
            return await _context.flats
                .Where(d => d.BlockId == blockid)
                .Select(d => new Flat
                {
                    FlatId = d.FlatId,
                    FlatNumber = d.FlatNumber,
                    NumberOfAdult = d.NumberOfAdult,
                    NumberOfChild = d.NumberOfChild,
                    TotalMember = d.TotalMember
                })
                .ToListAsync();
        }
        public async Task<List<FlatWithMembersDto>> getqr(int blockid)
        {
            var result = await _context.flats
                .Where(f => f.BlockId == blockid)
                .Select(f => new FlatWithMembersDto
                {
                    FlatId = f.FlatId,
                    FlatNumber = f.FlatNumber,
                    TotalMember = f.TotalMember,
                    Members = f.Members
                               .Where(m => m.IsActive)
                               .Select(m => new MemberDto
                               {
                                   MemberId = m.MemberId,
                                   QRCodeUrl = m.QRCodeUrl
                               })
                               .ToList()  
                })
                .ToListAsync();

            return result;
        }

    }
}
