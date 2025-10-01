using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(long userId);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User model);
        Task<string> Deleteuserasync(int userid);
        //UserRole
        Task<List<UserRole>> GetAllUserRoleAsync();
        Task<UserRole> GetRoleNameByIdAsync(long? id);
    }
}

