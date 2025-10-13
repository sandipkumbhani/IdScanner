using SocPass.Domain.Model;

namespace SocPass.UI.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUsersByIdAsync(int? id);
        Task<User> AddUserAsync(User user, int? flatId = null);
        Task<User> UpdateUserAsync(User user);
        Task<string> DeleteUserAsync(int id);
        //UserRole
        Task<List<UserRole>> GetAllUserRoleAsync();
        Task<UserRole> GetRoleNameByIdAsync(long? id);
    }
}
