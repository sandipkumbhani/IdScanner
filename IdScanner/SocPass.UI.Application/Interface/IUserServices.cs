using MicroService_Template.Domain.Model;

namespace Emertec.UI.Application.Interface
{
    public interface IUserServices
    {
        Task<List<ModelUsers>> GetAllUsersAsync();
        Task<ModelUsers?> GetUserByIdAsync(long userId);
        Task<ModelUsers> AddUserAsync(ModelUsers user);
        Task<ModelUsers> UpdateUserAsync(ModelUsers model);
        Task<string> Deleteuserasync(int userid);
        //UserRole
        Task<List<ModelUserRole>> GetAllUserRoleAsync();
        Task<ModelUserRole> GetRoleNameByIdAsync(long? id);
    }
}

