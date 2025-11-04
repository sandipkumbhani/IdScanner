using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Interface;
using SocPass.Infrastructure.Data;

namespace SocPass.Infrastructure.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;
        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }
        //var result = await _context.flats
        //        .Where(f => f.BlockId == blockid && f.IsActive)
        //        .Select(f => new FlatWithMembersDto
        //        {
        //            FlatId = f.FlatId,
        //            FlatNumber = f.FlatNumber,
        //            TotalMember = f.TotalMember,
        //            Members = f.Members
        //                .Where(m => m.IsActive && !m.IsGuest)
        //                .Select(m => new MemberDto
        //                {
        //                    MemberId = m.MemberId,
        //                    QRCodeUrl = _context.QRCodeMasters
        //                        .Where(q => q.MemberId == m.MemberId && q.EventId == eventId && q.IsActive)
        //                        .Select(q => q.QRCodeUrl)
        //                        .FirstOrDefault()
        //                })
        //                .Where(m => m.QRCodeUrl != null)
        //                .ToList()
        //        })
        //        .ToListAsync();

        //    return result;
        public async Task<object> GetFlatAttendanceReportAsync(int blockId, int eventId, DateTime startDate)
        {
            return await (
                from f in _context.flats
                join b in _context.blocks on f.BlockId equals b.BlockId
                join m in _context.members on f.FlatId equals m.FlatId
                join qr in _context.QRCodeMasters on m.MemberId equals qr.MemberId
                join e in _context.Events on qr.EventId equals e.EventId
                where b.BlockId == blockId && e.EventId == eventId && e.StartDate.Date == startDate.Date
                group new { f, m, qr } by new { f.FlatNumber, f.NumberOfAdult, f.NumberOfChild, f.TotalMember } into g
                select new
                {
                    g.Key.FlatNumber,
                    g.Key.NumberOfAdult,
                    g.Key.NumberOfChild,
                    g.Key.TotalMember,
                    VisitedAdult = g.Count(x => !x.m.IsChild && !x.m.IsGuest && x.qr.Visited),
                    VisitedChild = g.Count(x => x.m.IsChild && !x.m.IsGuest && x.qr.Visited),
                    PendingMember = g.Key.TotalMember - g.Count(x => !x.m.IsGuest && x.qr.Visited),
                    TotalGuest = g.Count(x => x.m.IsGuest),
                    VisitedGuest = g.Count(x => x.m.IsGuest && x.qr.Visited),
                    PendingGuest = g.Count(x => x.m.IsGuest && !x.qr.Visited)
                }
            ).ToListAsync();
        }

    }
}
