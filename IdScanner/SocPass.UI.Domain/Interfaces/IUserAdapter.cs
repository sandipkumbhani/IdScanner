using SocPass.Domain.Model;

namespace SocPass.UI.Domain.Interfaces
{
    public interface IUserAdapter
    {
        Task<IList<User>> GetAllUsersAsync();
        Task<User> GetUsersByIdAsync(int? id);
        Task<User> AddUserAsync(User user, int? flatId = null);
        Task<string> UpdateUserAsync(User user, int? flatId = null);
        Task<string> DeleteUserAsync(int id);
        //UserRole
        Task<IList<UserRole>> GetAllUserRoleAsync();
        Task<UserRole> GetRoleNameByIdAsync(long? id);
    }
}
