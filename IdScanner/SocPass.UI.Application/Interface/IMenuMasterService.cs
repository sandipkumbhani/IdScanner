using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IMenuMasterService
    {
        Task<List<MenuMaster>> GetAllMenuMasterAsync();
        Task<MenuMaster?> GetMenuByIdAsync(int menuId);
        Task<string> AddMenuAsync(MenuMaster modelMenuMaster);
        Task<string> UpdateMenuAsync(MenuMaster modelMenuMaster);
        Task<string> DeleteMenuAsync(int menuId);
        Task<List<MenuMaster>> GetMenusByUserIdAsync(int userId);
    }
}
