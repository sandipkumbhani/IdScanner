using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Interface
{
    public interface IMenuMasterService
    {
        Task<MenuMaster> CreateMenuMasterAsync(MenuMaster MenuMaster);
        Task<List<MenuMaster>> GetModelMenuMastersAsync();
        Task DeleteMenuById(int id);
        Task<MenuMaster> UpdateMenuAsync(int menuid, MenuMaster menuMaster);
        Task<MenuMaster> GetMenuMsaterById(int id);


    }
}
