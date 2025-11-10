using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Application.Services
{
    public class MenuMasterService : IMenuMasterService
    {
        private readonly IMenuMasterAdapter _menuMasterRepository;
        public MenuMasterService(IMenuMasterAdapter menuMasterRepository)
        {
            _menuMasterRepository = menuMasterRepository;
        }
        
        public async Task<IList<MenuMaster>> GetAllMenuMasterAsync()
        {
            return await _menuMasterRepository.GetAllMenuAsync();
        }

        public async Task<MenuMaster?>  GetMenuByIdAsync(int menuId)
        {
            return await _menuMasterRepository.GetMenuByIdAsync(menuId);
        }

        public async Task<string> AddMenuAsync(MenuMaster modelMenuMaster)
        {
            return await _menuMasterRepository.AddMenuAsync(modelMenuMaster);
        }

        public async Task<string> UpdateMenuAsync(MenuMaster modelMenuMaster)
        {
            return await _menuMasterRepository.UpdateMenuAsync(modelMenuMaster);
        }

        public async Task<string> DeleteMenuAsync(int menuId)
        {
             return await _menuMasterRepository.DeleteMenuAsync(menuId);  
        }
        //public async Task<IList<MenuMaster>> GetMenusByUserIdAsync(int userId)
        //{
        //     return await _menuMasterRepository.GetMenuByUserIdAsync(userId); 
        //}
    }
}
