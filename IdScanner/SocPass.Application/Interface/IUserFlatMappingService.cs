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
        Task<List<QRCodeMaster>> GetQrByUserId(int usereId, int? eventId = null);
    }
}
