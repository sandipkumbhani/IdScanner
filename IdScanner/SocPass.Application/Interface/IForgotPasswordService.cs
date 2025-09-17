using SocPass.Domain.Model;

namespace SocPass.Application.Interface
{
    public interface IForgotPasswordService
    {
        Task<User> CheckEmailidAsync(string email);
        Task<User> UpdatePasswordAsync(string email, User modelUsers);
    }
}
