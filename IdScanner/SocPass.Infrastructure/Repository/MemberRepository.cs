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
        public async Task<Member>AddMemberAsync(Member member)
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
    }
}
