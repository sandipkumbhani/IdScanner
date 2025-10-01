using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using System.Security.Claims;

namespace IdScanner.UI.Views.ViewComponents
{
    public class UserMenuViewComponent : ViewComponent
    {
        private readonly IMenuMasterService _menuService;

        public UserMenuViewComponent(IMenuMasterService menuService)
        {
            _menuService = menuService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(role))
                return View(new List<MenuMaster>());

            var menus = await _menuService.GetAllMenuMasterAsync();

            if (!role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                menus = menus.Where(m => !m.Name.Equals("Add Society", StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(menus);
        }

    }
}
