
using SocPass.Domain.Model;

namespace SocPass.Domain.Interface
{
    public interface IGetLoginUserNameRepository
    {
        Task<User> GetUserNameAsync(int userid);
    }
}
