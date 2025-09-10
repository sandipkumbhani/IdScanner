using IdScanner.Application.Interface;
using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Services
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
