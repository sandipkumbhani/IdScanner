using SocPass.UI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SocPass.UI.Domain.Interfaces
{
    public interface ILoginRepository
    {
        Task<ResponseToken> CreateUserLoginAsync(LoginViewModel userModel);
    }
}
