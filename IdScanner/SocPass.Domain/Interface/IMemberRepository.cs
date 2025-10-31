using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Interface
{
    public interface  IMemberRepository
    {
        Task<Member> AddMemberAsync(Member member);
        Task UpdateQrCodeAsync(int memberId, string qrCodeUrl);
        Task UpdateMemberAsync(Member member);
        Task<List<Member>> GetGuestsByFlatIdAsync(int flatId);
        Task<List<Member>> GetById(int flatId);
        Task DeleteMemberAsync(int memberId);
        Task<bool> AddMemberPassDateAsync(int blockId, int EventId);
        Task<Member> GetMemberByMemberIdAsync(int memberId, int EventId);
        Task<bool> IsVisitedAsync(int memberid, int loggedInUserId);
        Task<bool> AddGuestPassDateAsync(int blockId, DateTime passDate);
    }
}
