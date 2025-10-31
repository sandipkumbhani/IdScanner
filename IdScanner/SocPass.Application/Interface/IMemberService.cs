using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Interface
{
    public interface IMemberService
    {
        Task<List<Member>> GetGuestByIdAsync(int flatId);
        Task CreateAndUpdateGuestAsync(int flatId, int numberOfAdults, List<int> childAges);
        Task CreateAndUpdateMemberAsync(int flatId, int numberOfAdults, List<int> childAges);
        Task<List<Member>> GetMemberByIdAsync(int flatId);
        Task<bool> AddMemberPassDateAsync(int blockId, int EventId);
        Task<Member> GetMemberByMemberId(int memberId, int EventId);
        Task<bool> IsVisitedAsync(int memberid, int loggedInUserId);
        Task<bool> AddGuestPassDateAsync(int blockId, DateTime passDate);
    }
}
