using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Application.Services
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly IForgotPasswordRepository _forgotPasswordRepository;
        public ForgotPasswordService(IForgotPasswordRepository forgotPasswordRepository)
        {
            _forgotPasswordRepository = forgotPasswordRepository
                ?? throw new ArgumentNullException(nameof(forgotPasswordRepository));
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var emailid = await _forgotPasswordRepository.ForgotPasswordByEmailAsync(email);
            if(emailid == null)
            {
                throw new KeyNotFoundException($"User E-Mail ID {email} not found");
            }
            return emailid;
        }
    }
}
