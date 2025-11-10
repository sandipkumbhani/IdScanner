using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Interfaces
{
    public interface IMenuMasterAdapter
    {
        Task<IList<MenuMaster>> GetAllMenuAsync();
        Task<MenuMaster> GetMenuByIdAsync(int? id);
        Task<string> AddMenuAsync(MenuMaster menuMaster);
        Task<string> UpdateMenuAsync(MenuMaster menuMaster);
        Task<string> DeleteMenuAsync(int id);
        //Task<IList<MenuMaster>> GetMenuByUserIdAsync(int userId);
    }
}
