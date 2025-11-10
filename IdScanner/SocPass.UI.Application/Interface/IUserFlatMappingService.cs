using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IUserFlatMappingService
    {
        Task<IList<QRCodeMaster>> GetQrByUserId(int? userid,int EventId);
    }
}
