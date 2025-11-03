using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Interfaces
{
    public interface IMemberDetailsRepository
    {
        Task<bool> IsVisitedAsync(int memberid, int EventId, int loggedInUserId);
    }
}
