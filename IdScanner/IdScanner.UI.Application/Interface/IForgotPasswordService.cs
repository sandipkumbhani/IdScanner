using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Application.Interface
{
    public interface IForgotPasswordService
    {
        Task<string> ForgotPasswordAsync(string email);
    }
}
