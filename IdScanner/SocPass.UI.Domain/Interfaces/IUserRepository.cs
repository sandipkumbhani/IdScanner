using MicroService_Template.Domain.Model;

namespace Emertec.UI.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<ModelUsers>> GetAllUsersAsync();
        Task<ModelUsers> GetUsersByIdAsync(long? id);
        Task<ModelUsers> AddUserAsync(ModelUsers user);
        Task<ModelUsers> UpdateUserAsync(ModelUsers user);
        Task<string> DeleteUserAsync(int id);
        //UserRole
        Task<List<ModelUserRole>> GetAllUserRoleAsync();
        Task<ModelUserRole> GetRoleNameByIdAsync(long? id);
    }
}
