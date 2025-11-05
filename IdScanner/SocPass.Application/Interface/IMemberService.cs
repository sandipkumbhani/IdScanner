using SocPass.Domain.DTO;
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
        Task<string> AddMemberPassDateAsync(int blockId, int eventId);
        Task<QRCodeMaster> GetMemberByMemberId(int memberId, int EventId);
        Task<bool> IsVisitedAsync(int memberid, int EventId, int loggedInUserId);
        Task<string> GenerateGuestQRAsync(int blockId, int eventId);
    }
}
