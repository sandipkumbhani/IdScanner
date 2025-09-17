using SocPass.Application.Interface;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;

namespace SocPass.Application.Services
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly IForgotPasswordDbRepository _forgotPasswordRepository;
        public ForgotPasswordService(IForgotPasswordDbRepository forgotPasswordRepository)
        {
            _forgotPasswordRepository = forgotPasswordRepository
                ?? throw new ArgumentNullException(nameof(forgotPasswordRepository));
        }
        public async Task<User> CheckEmailidAsync(string email)
        {
            var emailid = await _forgotPasswordRepository.GetByEmailAsync(email);
            if (emailid == null)
            {
                throw new KeyNotFoundException($"User E-Mail ID {email} not found.");
            }

            return emailid;
        }
        public async Task<User> UpdatePasswordAsync(string email, User modelUsers)
        {
            var user = await _forgotPasswordRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException($"User E-Mail ID {email} not found.");
            }
            user.Password =modelUsers.Password;
            await _forgotPasswordRepository.UpdateAsync(user);

            return user;
        }
    }
}
