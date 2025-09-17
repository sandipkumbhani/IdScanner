
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Application.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly ILoginRepository _loginRepository;
        public LoginServices(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<ResponseToken> Login(LoginViewModel userModel)
        {
            return await _loginRepository.CreateUserLoginAsync(userModel);
        }

    }
}
