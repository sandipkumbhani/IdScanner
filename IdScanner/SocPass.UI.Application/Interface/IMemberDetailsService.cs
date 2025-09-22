using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Application.Interface
{
    public interface IMemberDetailsService
    {
        Task<bool> IsVisitedAsync(int memberid, int loggedInUserId);
    }
}
