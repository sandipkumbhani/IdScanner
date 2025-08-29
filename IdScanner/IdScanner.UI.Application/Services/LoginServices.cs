using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.UI.Application.Interface;
using IdScanner.UI.Domain.Interfaces;
using IdScanner.UI.Domain.Model;

namespace IdScanner.UI.Application.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly ILoginRepository _loginRepository;
        public LoginServices(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<ResponseToken> Login(LoginViewModel model)
        {
            return await _loginRepository.CreateUserLoginAsync(model);
        }

    }
}
