using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Application.Services
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IResetPasswordAdapter _resetPasswordRepository;
        public ResetPasswordService(IResetPasswordAdapter resetPasswordRepository)
        {
            _resetPasswordRepository = resetPasswordRepository;
        }
        public async Task<string> ResetPassworsdAsync(ResetPasswordModel resetPasswordModel)
        {
            return await _resetPasswordRepository.UpdatePasswordAsync(resetPasswordModel);
        }
    }
}
