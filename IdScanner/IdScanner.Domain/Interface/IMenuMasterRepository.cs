using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Interface
{
    public interface IMenuMasterRepository
    {
        Task<MenuMaster> AddMenuMasterAsync(MenuMaster menuMaster);
        Task<List<MenuMaster>> GetAllMenuAsync();
        Task<MenuMaster> GetMenuById(int menuid);
        Task DeleteMenuAsync(MenuMaster modelMenuMaster);
        Task UpdatMenuAsync(MenuMaster menuMaster);
    }
}
