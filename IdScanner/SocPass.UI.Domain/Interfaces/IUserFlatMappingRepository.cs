using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Interfaces
{
    public interface IUserFlatMappingRepository
    {
        Task<List<QRCodeMaster>> GetQrByUserId(int? userid, int EventId);
    }
}
