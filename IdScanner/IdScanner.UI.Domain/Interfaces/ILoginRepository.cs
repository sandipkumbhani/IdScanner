using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;
using IdScanner.UI.Domain.Model;

namespace IdScanner.UI.Domain.Interfaces
{
    public interface ILoginRepository
    {
        Task<ResponseToken> CreateUserLoginAsync(LoginViewModel userModel);
    }
}
