using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.UI.Domain.Model;

namespace IdScanner.UI.Application.Interface
{
    public interface ILoginServices
    {
        Task<ResponseToken> Login(LoginViewModel model);
    }
}
