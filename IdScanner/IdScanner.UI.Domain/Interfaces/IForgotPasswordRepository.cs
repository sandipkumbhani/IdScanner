using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Domain.Interfaces
{
    public interface IForgotPasswordRepository
    {
        Task<string> ForgotPasswordByEmailAsync(string email);
    }
}
