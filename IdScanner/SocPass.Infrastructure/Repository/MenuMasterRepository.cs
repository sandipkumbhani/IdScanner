using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Infrastructure.Repository
{
    public class MenuMasterRepository : IMenuMasterRepository
    {
        private AppDbContext _context;
        public MenuMasterRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<MenuMaster> AddMenuMasterAsync(MenuMaster menuMaster)
        {
            _context.MenuMasters.Add(menuMaster);
            await _context.SaveChangesAsync();
            return menuMaster;
        }
        public async Task<List<MenuMaster>> GetAllMenuAsync()
        {
            return await _context.MenuMasters.Where(u => u.IsActive).OrderBy(x=>x.Name).ToListAsync();
        }
        public async Task<MenuMaster> GetMenuById(int menuid)
        {
            return _context.MenuMasters
                .FirstOrDefault(e => e.MenuId == menuid);
        }
        public async Task DeleteMenuAsync(int menuId)
        {
            var existingMenu = await _context.MenuMasters.FindAsync(menuId);
            if (existingMenu != null)
            {
                existingMenu.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdatMenuAsync(MenuMaster menuMaster)
        {
            _context.MenuMasters.Update(menuMaster);
            _context.SaveChanges();
        }
        //public async Task<List<MenuMaster>> GetMenusByUserIdAsync(long userId)
        //{
        //    var menus = await (from um in _context.MenuMasters
        //                       join m in _context.MenuMasters on um.MenuId equals m.MenuId
        //                       where um.UserId == userId
        //                             && um.IsActive
        //                             && m.IsActive
        //                             && m.IsDefault
        //                       select m)
        //              .Distinct()
        //              .ToListAsync();

        //    return menus;
        //}
    }
}
