using SocPass.Application.Interface;
using SocPass.Domain.Model;

namespace SocPass.Application.Services
{
    public class GetLoginUserNameService : IGetLoginUserNameService
    {
        private readonly IGetLoginUserNameRepository _getLoginUserNameRepository;
        public GetLoginUserNameService(IGetLoginUserNameRepository getLoginUserNameRepository)
        {
            _getLoginUserNameRepository = getLoginUserNameRepository;
        }
        public async Task<User> GetLoginUserNameAsync(long userId)
        {
            return await _getLoginUserNameRepository.GetUserNameAsync(userId);
        }
    }
}
