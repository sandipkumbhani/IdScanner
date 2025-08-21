using IdScanner.Application.Interface;
using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Services
{
    public class MenuMasterService : IMenuMasterService
    {
        private readonly IMenuMasterRepository _menuMasterRepository;
        public MenuMasterService(IMenuMasterRepository MenuMasterRepository)
        {
            _menuMasterRepository = MenuMasterRepository;
        }
        public async Task<MenuMaster> CreateMenuMasterAsync(MenuMaster MenuMaster)
        {
            var menuMaster = new MenuMaster
            {
                Name = MenuMaster.Name,
                Description = MenuMaster.Description,
                Icon = MenuMaster.Icon,
                Url = MenuMaster.Url,
                IsDefault = MenuMaster.IsDefault,
                IsActive = true,
                InsertBy = 1,
                InsertDate = DateTime.Now,
                UpdateBy = 1,
                UpdateDate = DateTime.Now
            };

            return await _menuMasterRepository.AddMenuMasterAsync(menuMaster);
        }
        public async Task<List<MenuMaster>> GetModelMenuMastersAsync()
        {
            var users = await _menuMasterRepository.GetAllMenuAsync();

            return users.Select(menuMaster => new MenuMaster
            {
                MenuId = menuMaster.MenuId,
                Description = menuMaster.Description,
                Name = menuMaster.Name,
                Icon = menuMaster.Icon,
                Url = menuMaster.Url,
                IsDefault = menuMaster.IsDefault,
                IsActive = menuMaster.IsActive,
                InsertBy = menuMaster.InsertBy,
                InsertDate = menuMaster.InsertDate,
                UpdateBy = menuMaster.UpdateBy,
                UpdateDate = menuMaster.UpdateDate,
            }).ToList();
        }
        public async Task DeleteMenuById(int id)
        {
            var deleteMenu = await _menuMasterRepository.GetMenuById(id);
            if (deleteMenu == null)
            {
                throw new KeyNotFoundException($"Menu Master ID {id} not found.");
            }

            await _menuMasterRepository.DeleteMenuAsync(deleteMenu);
        }
        public async Task<MenuMaster> UpdateMenuAsync(int menuid, MenuMaster menuMaster)
        {

            var menuExisting = await _menuMasterRepository.GetMenuById(menuid);

            if (menuExisting == null)
            {
                throw new Exception($"Menu Master with ID {menuid} not found.");
            }
            menuExisting.Name = menuMaster.Name;
            menuExisting.Description = menuMaster.Description;
            menuExisting.Icon = menuMaster.Icon;
            menuExisting.Url = menuMaster.Url;
            menuExisting.IsDefault = true;
            menuExisting.IsActive = true;
            menuExisting.InsertBy = 1;
            menuExisting.InsertDate = DateTime.UtcNow;
            menuExisting.UpdateBy = 1;
            menuExisting.UpdateDate = DateTime.UtcNow;


            await _menuMasterRepository.UpdatMenuAsync(menuExisting);

            return menuExisting;
        }
        public async Task<MenuMaster> GetMenuMsaterById(int id)
        {
            var menuMaster = await _menuMasterRepository.GetMenuById(id);
            if (menuMaster == null)
            {
                throw new KeyNotFoundException($"Menu Mater with ID {id} not found.");
            }

            return menuMaster;
        }
    }
}
