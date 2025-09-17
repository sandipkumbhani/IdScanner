using SocPass.Domain.Model;

namespace SocPass.Domain.Interface
{
    public interface IForgotPasswordDbRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
    }
}
