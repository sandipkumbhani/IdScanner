using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;
using IdScanner.UI.Domain.Interfaces;

namespace IdScanner.UI.Application.Services
{
    public class MenuMasterService : IMenuMasterService
    {
        private readonly IMenuMasterRepository _menuMasterRepository;
        public MenuMasterService(IMenuMasterRepository menuMasterRepository)
        {
            _menuMasterRepository = menuMasterRepository;
        }
        
        public async Task<List<MenuMaster>> GetAllMenuMasterAsync()
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
        public async Task<List<MenuMaster>> GetMenusByUserIdAsync(long userId)
        {
             return await _menuMasterRepository.GetMenuByUserIdAsync(userId); 
        }
    }
}
