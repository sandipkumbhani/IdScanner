using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IUserFlatMappingService
    {
        Task<List<Member>> GetQrByUserId(int? userid);
    }
}
