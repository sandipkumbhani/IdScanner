using IdScanner.UI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Application.Interface
{
    public interface IResetPasswordService
    {
        Task<string> ResetPassworsdAsync(ResetPasswordModel resetPasswordModel);
    }
}
