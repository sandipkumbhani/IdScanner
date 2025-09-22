using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Application.Services
{
    public class MemberDetailsService : IMemberDetailsService
    {
        public readonly IMemberDetailsRepository _memberDetailsRepository;
        public MemberDetailsService(IMemberDetailsRepository memberDetailsRepository)
        {
            _memberDetailsRepository = memberDetailsRepository;
        }

        public async Task<bool> IsVisitedAsync(int memberid, int loggedInUserId)
        {
            return await _memberDetailsRepository.IsVisitedAsync(memberid, loggedInUserId);
        }
    }
}
