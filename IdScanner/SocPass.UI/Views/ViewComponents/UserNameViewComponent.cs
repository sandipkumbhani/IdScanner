using Microsoft.AspNetCore.Mvc;
using SocPass.UI.Application.Interface;
using System.Security.Claims;

namespace SocPass.UI.Views.ViewComponents
{
    public class UserNameViewComponent : ViewComponent
    {
        private readonly IGetUserNameByIdService _getUserNameByIdService;
        public UserNameViewComponent(IGetUserNameByIdService getUserNameByIdService)
        {
            _getUserNameByIdService = getUserNameByIdService ?? throw new ArgumentNullException(nameof(getUserNameByIdService));
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            ViewBag.role = role;
            int.TryParse(HttpContext.User?.FindFirst("UserId")?.Value, out int userId);
            var user = await _getUserNameByIdService.GetUserNameByIdAsync(userId);

            return View("Default", user);
        }

    }

}
