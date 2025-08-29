using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;

namespace IdScanner.UI.Application.Interface
{
    public interface IMenuMasterService
    {
        Task<List<MenuMaster>> GetAllMenuMasterAsync();
        Task<MenuMaster?> GetMenuByIdAsync(int menuId);
        Task<string> AddMenuAsync(MenuMaster modelMenuMaster);
        Task<string> UpdateMenuAsync(MenuMaster modelMenuMaster);
        Task<string> DeleteMenuAsync(int menuId);
        Task<List<MenuMaster>> GetMenusByUserIdAsync(long userId);
    }
}
