using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;

namespace IdScanner.UI.Domain.Interfaces
{
    public interface IMenuMasterRepository
    {
        Task<List<MenuMaster>> GetAllMenuAsync();
        Task<MenuMaster> GetMenuByIdAsync(int? id);
        Task<string> AddMenuAsync(MenuMaster menuMaster);   
        Task<string> UpdateMenuAsync(MenuMaster menuMaster);
        Task<string> DeleteMenuAsync(int id);
        Task<List<MenuMaster>> GetMenuByUserIdAsync(long userId);

    }
}
