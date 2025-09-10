using IdScanner.UI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Domain.Interfaces
{
    public interface IResetPasswordRepossitory
    {
        Task<string> UpdatePasswordAsync(ResetPasswordModel resetPasswordModel);
    }
}
