using SocPass.Domain.Model;

namespace SocPass.Application.Interface
{
    public interface IGetLoginUserNameService
    {
        Task<User> GetLoginUserNameAsync(int userId);
    }
}
