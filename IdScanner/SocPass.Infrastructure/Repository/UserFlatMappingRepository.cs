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
        public async Task<List<QRCodeMaster>> GetQrByUserId(int userId, int? eventId = null)
        {
            var qrList = await (from ufm in _context.userFlatMappings
                                where ufm.UserId == userId && ufm.IsActive
                                let flatId = ufm.FlatId
                                where flatId != 0
                                join m in _context.members on flatId equals m.FlatId
                                where m.IsActive
                                select m.MemberId)
                 .Distinct()
                 .Join(_context.QRCodeMasters.Where(q => q.IsActive),
                       memberId => memberId,
                       qr => qr.MemberId,
                       (memberId, qr) => qr)
                 .Include(q => q.Member)
                 .Include(q => q.Event)
                 .ToListAsync();

            if (eventId.HasValue && eventId.Value != 0)
            {
                qrList = qrList.Where(q => q.EventId == eventId.Value).ToList();
            }

            return qrList;
        }

        public async Task<UserFlatMapping> GetMappingByUserId(int userid)
        {
            return await _context.userFlatMappings
                .FirstOrDefaultAsync(e => e.UserId == userid && e.IsActive == true);
        }
        public async Task<UserFlatMapping> GetMappingByFlatId(int? flatId)
        {
            return await _context.userFlatMappings
                .FirstOrDefaultAsync(e => e.FlatId == flatId && e.IsActive == true);
        }
        public async Task UpdateFlatMappingAsync(UserFlatMapping userFlatMapping)
        {
            _context.userFlatMappings.Update(userFlatMapping);
            await _context.SaveChangesAsync();
        }

    }
}
