using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IUserFlatMappingService
    {
        Task<List<QRCodeMaster>> GetQrByUserId(int? userid,int EventId);
    }
}
