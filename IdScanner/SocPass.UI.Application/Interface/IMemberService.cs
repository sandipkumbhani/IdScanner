
using Microsoft.Extensions.Logging;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Application.Interface
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllMember(int flatId);
        Task<string> AddMemberAsync(MemberCreateRequest memberCreateRequest);
        Task<string> GeneratePass(int blockId, int EventId);
        Task<List<Member>> GetAllMemberAsync(int flatId);
        Task<Member> GetMemberByMemberId(int memberId, int EventId);
        Task<string> AddAndUpdateGuestAsync(MemberCreateRequest memberCreateRequest);
        Task<string> GenerateGuestPass(int blockId, DateTime passDate);
    }
}
