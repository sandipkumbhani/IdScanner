using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Interface
{
    public interface IUserFlatMappingService
    {
        Task<List<Member>> GetQrByUserId(int usereId);
    }
}
