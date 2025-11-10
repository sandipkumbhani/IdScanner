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
        public readonly IMemberDetailsAdapter _memberDetailsRepository;
        public MemberDetailsService(IMemberDetailsAdapter memberDetailsRepository)
        {
            _memberDetailsRepository = memberDetailsRepository;
        }

        public async Task<string> IsVisitedAsync(int memberid,int EventId, int loggedInUserId)
        {
            return await _memberDetailsRepository.IsVisitedAsync(memberid, EventId ,loggedInUserId);
        }

    }
}
