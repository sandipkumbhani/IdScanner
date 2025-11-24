using Microsoft.EntityFrameworkCore;
using SocPass.Domain.DTO;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;

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
        public async Task<List<Flat>> GetFlatAsync()
        {
            return await _context.flats.Include(X => X.Society).Include(X => X.Block).OrderBy(X => X.Society.Name).ThenBy(X => X.Block.BlockNumber).ThenBy(X => X.FloorNumber).ThenBy(X => X.FlatNumber).Where(X => X.IsActive == true).ToListAsync();
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
        public async Task<List<Flat>> GetFlatsByBlockAsync(int societyId, int blockId)
        {
            return await _context.flats
                .Where(f => f.SocietyId == societyId && f.BlockId == blockId)
                .ToListAsync() ?? new List<Flat>();
        }
        public async Task DeleteFlatAsync(int flatId)
        {
            var member = await _context.flats.FindAsync(flatId);
            if (member != null)
            {
                _context.flats.Remove(member);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Flat>> GetFlatByBlockIdAsync(int blockid)
        {
            return await _context.flats
                .Where(d => d.BlockId == blockid && d.IsActive==true)
                .Select(d => new Flat
                {
                    FlatId = d.FlatId,
                    FlatNumber = d.FlatNumber,
                    NumberOfAdult = d.NumberOfAdult,
                    NumberOfChild = d.NumberOfChild,
                    TotalMember = d.TotalMember,
                    SocietyId=d.SocietyId
                    
                })
                .ToListAsync();
        }
        public async Task<List<FlatWithMembersDto>> GetMemberQrAsync(int blockid, int eventId)
        {
            var result = await _context.flats
                .Where(f => f.BlockId == blockid && f.IsActive)
                .Select(f => new FlatWithMembersDto
                {
                    FlatId = f.FlatId,
                    FlatNumber = f.FlatNumber,
                    TotalMember = f.TotalMember,
                    Members = f.Members
                        .Where(m => m.IsActive && !m.IsGuest)
                        .Select(m => new MemberDto
                        {
                            MemberId = m.MemberId,
                            QRCodeUrl = _context.QRCodeMasters
                                .Where(q => q.MemberId == m.MemberId && q.EventId == eventId && q.IsActive)
                                .Select(q => q.QRCodeUrl)
                                .FirstOrDefault()
                        })
                        .Where(m => m.QRCodeUrl != null)
                        .ToList()
                })
                .ToListAsync();

            return result;
        }
        public async Task<List<FlatWithMembersDto>> GetGuestQr(int blockid, int EventId)
        {
            var result = await _context.flats
                .Where(f => f.BlockId == blockid && f.IsActive)
                .Select(f => new FlatWithMembersDto
                {
                    FlatId = f.FlatId,
                    FlatNumber = f.FlatNumber,
                    TotalMember = f.TotalMember,
                    Members = f.Members
                        .Where(m => m.IsActive && m.IsGuest)
                        .Select(m => new MemberDto
                        {
                            MemberId = m.MemberId,
                            QRCodeUrl = _context.QRCodeMasters
                                .Where(q => q.MemberId == m.MemberId && q.EventId == EventId && q.IsActive)
                                .Select(q => q.QRCodeUrl)
                                .FirstOrDefault()
                        })
                        .Where(m => m.QRCodeUrl != null)
                        .ToList()
                })
                .ToListAsync();

            return result;
        }

    }
}
