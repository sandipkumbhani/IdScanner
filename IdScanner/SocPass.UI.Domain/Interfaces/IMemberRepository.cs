using SocPass.Domain.Model;
using SocPass.UI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Interfaces
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetAllGuestAsync(int flatId);
        Task<List<Member>> GetAllMemberAsync(int flatId);
        Task<string> AddMemberAsync(MemberCreateRequest memberCreateRequest);
        Task<string> GeneratePass(int blockId, int EventId);
        Task<QRCodeMaster> GetMemberByMemberId(int? memberId,int EventId);
        Task<string> AddAndUpdateGuestAsync(MemberCreateRequest memberCreateRequest);
        Task<string> GenerateGuestPass(int blockId, int EventId);
    }
}
