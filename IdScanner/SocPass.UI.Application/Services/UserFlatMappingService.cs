using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Application.Services
{
    public class UserFlatMappingService : IUserFlatMappingService
    {
        private readonly IUserFlatMappingRepository _userFlatMappingRepository;
        public UserFlatMappingService(IUserFlatMappingRepository userFlatMappingRepository)
        {
            _userFlatMappingRepository = userFlatMappingRepository;
        }
        public async Task<List<QRCodeMaster>> GetQrByUserId(int? userid,int EventId)
        {
            return await _userFlatMappingRepository.GetQrByUserId(userid, EventId);
        }
    }
}
