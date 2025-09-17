using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IGetUserNameByIdService
    {
        Task<User> GetUserNameByIdAsync(int userId);
    }
}
