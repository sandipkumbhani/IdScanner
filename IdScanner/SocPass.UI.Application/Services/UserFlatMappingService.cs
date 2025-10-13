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
        public async Task<List<Member>> GetQrByUserId(int? userid)
        {
            return await _userFlatMappingRepository.GetQrByUserId(userid);
        }
    }
}
