using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface  IUserService
    {
        Task<IList<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task<User> AddUserAsync(User user, int? flatId = null);
        Task<string> UpdateUserAsync(User model, int? flatId = null);
        Task<string> Deleteuserasync(int userid);
        //UserRole
        Task<IList<UserRole>> GetAllUserRoleAsync();
        Task<UserRole> GetRoleNameByIdAsync(long? id);
    }
}

