using IdScanner.UI.Application.Interface;
using IdScanner.UI.Domain.Interfaces;
using IdScanner.UI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Application.Services
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IResetPasswordRepossitory _resetPasswordRepository;
        public ResetPasswordService(IResetPasswordRepossitory resetPasswordRepository)
        {
            _resetPasswordRepository = resetPasswordRepository;
        }
        public async Task<string> ResetPassworsdAsync(ResetPasswordModel resetPasswordModel)
        {
            return await _resetPasswordRepository.UpdatePasswordAsync(resetPasswordModel);
        }
    }
}
