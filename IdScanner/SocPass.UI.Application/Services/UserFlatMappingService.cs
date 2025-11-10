using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Application.Services
{
    public class UserFlatMappingService : IUserFlatMappingService
    {
        private readonly IUserFlatMappingAdapter _userFlatMappingAdapter;
        public UserFlatMappingService(IUserFlatMappingAdapter userFlatMappingAdapter)
        {
            _userFlatMappingAdapter = userFlatMappingAdapter;
        }
        public async Task<IList<QRCodeMaster>> GetQrByUserId(int? userid,int EventId)
        {
            return await _userFlatMappingAdapter.GetQrByUserId(userid, EventId);
        }
    }
}
