using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;
using Microsoft.AspNetCore.Mvc;

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
            //var userIdClaim = HttpContext.User.FindFirst("UserId")?.Value;
            //if (!long.TryParse(userIdClaim, out long userId))
            //{
            //    return View(new List<MenuMaster>());
            //}

            //var menus = await _menuService.GetMenusByUserIdAsync(Convert.ToInt64(userIdClaim));
            //return View(menus);
            var menus = await _menuService.GetAllMenuMasterAsync();
            return View(menus);
        }
    }
}
