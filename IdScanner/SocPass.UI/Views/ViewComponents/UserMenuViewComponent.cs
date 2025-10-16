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
            {
                return View(new List<MenuMaster>());
            }
            var menus = await _menuService.GetAllMenuMasterAsync();

            if (!role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                var excludedMenus = new List<string> { "Add Society", "Add Subscription", "Add User", "AppSetting" };

                menus = menus
                    .Where(m => !excludedMenus.Contains(m.Name, StringComparer.OrdinalIgnoreCase))
                    .ToList();
            }
            return View(menus);
        }

    }
}
