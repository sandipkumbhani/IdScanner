using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SocPass.Domain.DTO;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;

namespace SocPass.Infrastructure.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;
        private readonly AppSettingsDTO _baseUrl;
        public MemberRepository(AppDbContext context, IOptions<AppSettingsDTO> baseUrl)
        {
            _baseUrl = baseUrl.Value;
            _context = context;
        }
        public async Task<Member> AddMemberAsync(Member member)
        {
            _context.members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }
        public async Task<List<Member>> GetGuestsByFlatIdAsync(int flatId)
        {
            return await _context.members
                .Where(x => x.FlatId == flatId && x.IsGuest == true)
                .ToListAsync();
        }
        public async Task<List<Member>> GetById(int flatId)
        {
            return await _context.members
                .Where(x => x.IsActive && !x.IsGuest && x.FlatId == flatId)
                .ToListAsync();
        }
        public async Task UpdateMemberAsync(Member member)
        {
            _context.members.Update(member);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMemberAsync(int memberId)
        {
            var member = await _context.members.FindAsync(memberId);
            if (member != null)
            {
                member.IsActive = false;
                await _context.SaveChangesAsync();
            }
            ;
        }
        public async Task<QRCodeMaster?> GetMemberByMemberIdAsync(int memberId, int eventId)
        {
            return await _context.QRCodeMasters
                .Where(x => x.MemberId == memberId && x.EventId == eventId && x.IsActive)
                .Select(m => new QRCodeMaster
                {
                    QRCodeId = m.QRCodeId,
                    MemberId = m.MemberId,
                    EventId = m.EventId,
                    Visited = m.Visited,
                    IsActive = m.IsActive,

                    Event = m.Event != null ? new Event
                    {
                        EventId = m.Event.EventId,
                        EventName = m.Event.EventName,
                         StartDate = m.Event.StartDate
                    } : null,

                    Member = m.Member != null ? new Member
                    {
                        MemberId = m.Member.MemberId,
                        IsChild = m.Member.IsChild,
                        ChildAge = m.Member.ChildAge,
                        IsGuest = m.Member.IsGuest,
                        FlatId = m.Member.FlatId,
                        Flat = m.Member.Flat != null ? new Flat
                        {
                            FlatId = m.Member.Flat.FlatId,
                            FlatNumber = m.Member.Flat.FlatNumber,
                            TotalMember = m.Member.Flat.TotalMember,
                            NumberOfChild = m.Member.Flat.NumberOfChild,
                            NumberOfAdult = m.Member.Flat.NumberOfAdult,
                            Block = m.Member.Flat.Block != null ? new Block
                            {
                                BlockId = m.Member.Flat.Block.BlockId,
                                BlockNumber = m.Member.Flat.Block.BlockNumber
                            } : null,
                            Society = m.Member.Flat.Society != null ? new Society
                            {
                                SocietyId = m.Member.Flat.Society.SocietyId,
                                Name = m.Member.Flat.Society.Name
                            } : null
                        } : null
                    } : null
                })
                .FirstOrDefaultAsync();
        }
        public async Task<string> IsVisitedAsync(int memberId, int eventId, int loggedInUserId)
        {
            var entity = await _context.QRCodeMasters
                .Include(e => e.Event)
                .FirstOrDefaultAsync(x => x.MemberId == memberId && x.EventId == eventId);

            if (entity == null)
                return "NotFound";

            var today = DateTime.Now.Date;
            var eventDate = entity.Event.StartDate.Date;

            if (entity.Visited)
                return "AlreadyVisited";

            if (eventDate != today)
            {
                return "InvalidDate";
            }

            entity.Visited = true;
            entity.UpdateDate = DateTime.Now;
            entity.UpdateBy = loggedInUserId;

            await _context.SaveChangesAsync();
            return "Visited";
        }

        public async Task<Tuple<IList<QRCodeMaster>, IList<Flat>, IList<Member>>> GenerateGuestQRAsync(int blockId, int eventId)
        {
            var flatIds = await _context.flats
                 .Where(f => f.BlockId == blockId && f.IsActive)
                 .Select(f => f.FlatId)
                 .ToListAsync();

            var members = await _context.members
                .Where(m => flatIds.Contains(m.FlatId) && m.IsActive && m.IsGuest)
                .ToListAsync();

            var existingQRGenerated = await _context.QRCodeMasters
                .Where(q => q.EventId == eventId && q.IsActive)
                .ToListAsync();

            var flatsList = await _context.flats
                .Include(f => f.Block)
                .ThenInclude(b => b.Society)
                .ToListAsync();

            return new Tuple<IList<QRCodeMaster>, IList<Flat>, IList<Member>>(existingQRGenerated, flatsList, members);
        }
        public async Task<Tuple<IList<QRCodeMaster>,IList<Flat>,IList<Member>>> AddMemberPassDateAsync(int blockId, int eventId)
        {
            var flatIds = await _context.flats
                .Where(f => f.BlockId == blockId && f.IsActive)
                .Select(f => f.FlatId)
                .ToListAsync();

            var members = await _context.members
                .Where(m => flatIds.Contains(m.FlatId) && m.IsActive && !m.IsGuest)
                .ToListAsync();

            var existingQRGenerated = await _context.QRCodeMasters
                .Where(q => q.EventId == eventId && q.IsActive)
                .ToListAsync();

            var flatsList = await _context.flats
                .Include(f => f.Block)
                .ThenInclude(b => b.Society)
                .ToListAsync();

            return new Tuple<IList<QRCodeMaster>, IList<Flat>, IList<Member>>(existingQRGenerated, flatsList, members);
        }

        public async Task AddQrMasterAsync(IEnumerable<QRCodeMaster> qrList)
        {
            await _context.QRCodeMasters.AddRangeAsync(qrList);
            await _context.SaveChangesAsync();
        }

     
    }
}


