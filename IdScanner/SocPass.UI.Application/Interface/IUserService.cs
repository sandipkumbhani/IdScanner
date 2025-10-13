using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task<User> AddUserAsync(User user, int? flatId = null);
        Task<User> UpdateUserAsync(User model);
        Task<string> Deleteuserasync(int userid);
        //UserRole
        Task<List<UserRole>> GetAllUserRoleAsync();
        Task<UserRole> GetRoleNameByIdAsync(long? id);
    }
}

