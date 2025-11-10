using SocPass.Domain.DTO;
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
        //Task UpdateQrCodeAsync(int memberId, string qrCodeUrl);
        Task UpdateMemberAsync(Member member);
        Task<List<Member>> GetGuestsByFlatIdAsync(int flatId);
        Task<List<Member>> GetById(int flatId);
        Task DeleteMemberAsync(int memberId);
        Task<Tuple<IList<QRCodeMaster>, IList<Flat>, IList<Member>>> AddMemberPassDateAsync(int blockId, int eventId);
        //Task<bool> AddMemberPassDateAsync(int blockId, int EventId);
        Task<string> IsVisitedAsync(int memberId, int eventId, int loggedInUserId);
        Task<Tuple<IList<QRCodeMaster>, IList<Flat>, IList<Member>>> GenerateGuestQRAsync(int blockId, int eventId);
        Task<QRCodeMaster?> GetMemberByMemberIdAsync(int memberId, int eventId);
        Task AddQrMasterAsync(IEnumerable<QRCodeMaster> qrList);
    }
}

