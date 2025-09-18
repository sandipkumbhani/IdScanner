using SocPass.Domain.DTO;
using SocPass.Domain.Model;
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
    }
}
