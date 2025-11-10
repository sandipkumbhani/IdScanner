using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IMenuMasterService
    {
        Task<IList<MenuMaster>> GetAllMenuMasterAsync();
        Task<MenuMaster> GetMenuByIdAsync(int menuId);
        Task<string> AddMenuAsync(MenuMaster modelMenuMaster);
        Task<string> UpdateMenuAsync(MenuMaster modelMenuMaster);
        Task<string> DeleteMenuAsync(int menuId);
        //Task<IList<MenuMaster>> GetMenusByUserIdAsync(int userId);
    }
}
