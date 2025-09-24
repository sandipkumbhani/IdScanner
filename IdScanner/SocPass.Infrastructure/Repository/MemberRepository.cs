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
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;
        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Member> AddMemberAsync(Member member)
        {
            _context.members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }
        public async Task UpdateQrCodeAsync(int memberId, string qrCodeUrl)
        {
            var user = await _context.members.FindAsync(memberId);
            if (user != null)
            {
                user.QRCodeUrl = qrCodeUrl;
                _context.members.Update(user);
                await _context.SaveChangesAsync();
            }
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
            };
        }
        public async Task<Member> GetMemberByMemberIdAsync(int memberId)
        {
            return await _context.members
                .Where(x => x.MemberId == memberId && x.IsActive)
                .Select(m => new Member
                {
                    MemberId = m.MemberId,
                    IsChild = m.IsChild,
                    ChildAge = m.ChildAge,
                    IsGuest = m.IsGuest,
                    Visited = m.Visited,
                    FlatId = m.FlatId,
                    Flat = m.Flat != null ? new Flat
                    {
                        FlatId = m.Flat.FlatId,
                        FlatNumber = m.Flat.FlatNumber,
                        TotalMember = m.Flat.TotalMember,
                        NumberOfChild = m.Flat.NumberOfChild,
                        NumberOfAdult = m.Flat.NumberOfAdult,
                        Block = m.Flat.Block != null ? new Block
                        {
                            BlockId = m.Flat.Block.BlockId,
                            BlockNumber = m.Flat.Block.BlockNumber
                        } : null,
                        Society = m.Flat.Society != null ? new Society
                        {
                            SocietyId = m.Flat.Society.SocietyId,
                            Name = m.Flat.Society.Name
                        } : null
                    } : null
                })
                .FirstOrDefaultAsync();
        }


        public async Task<bool> IsVisitedAsync(int memberid, int loggedInUserId)
        {
            //var entity = await _context.members
            //    .FirstOrDefaultAsync(x => x.MemberId == memberid && x.Visited == false);
            //if (entity == null)
            //{
            //    return false;
            //}

            //entity.Visited = true;
            //entity.UpdateDate = DateTime.Now;
            //entity.UpdateBy = loggedInUserId;
            //await _context.SaveChangesAsync();
            //return true;

            var entity = await _context.members
    .FirstOrDefaultAsync(x => x.MemberId == memberid);

            if (entity == null)
                return false;

            if (!entity.Visited) // update only if not visited
            {
                entity.Visited = true;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateBy = loggedInUserId;
                await _context.SaveChangesAsync();
            }
            return true;

        }

        public async Task<bool> AddMemberPassDateAsync(int blockId, DateTime passDate)
        {
            var flatIds = await _context.flats
                                        .Where(f => f.BlockId == blockId && f.IsActive==true)
                                        .Select(f => f.FlatId)
                                        .ToListAsync();

            if (!flatIds.Any())
            {
                return false;
            }

            var members = await _context.members
                                        .Where(m => flatIds.Contains(m.FlatId) && m.IsActive == true &&m.IsGuest == false)
                                        .ToListAsync();

            if (!members.Any())
            {
                return false;
            }

            foreach (var member in members)
            {
                member.PassDate = DateOnly.FromDateTime(passDate);
                member.Visited = false;
                member.UpdateDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddGuestPassDateAsync(int blockId, DateTime passDate)
        {
            var flatIds = await _context.flats
                                        .Where(f => f.BlockId == blockId && f.IsActive == true)
                                        .Select(f => f.FlatId)
                                        .ToListAsync();

            if (!flatIds.Any())
            {
                return false;
            }

            var members = await _context.members
                                        .Where(m => flatIds.Contains(m.FlatId) && m.IsActive == true && m.IsGuest == true)
                                        .ToListAsync();

            if (!members.Any())
            {
                return false;
            }

            foreach (var member in members)
            {
                member.PassDate = DateOnly.FromDateTime(passDate);
                member.Visited = false;
                member.UpdateDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

        
