using IdScanner.UI.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdScanner.UI.Views.ViewComponents
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
            string userName = null;

            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            long.TryParse(userIdClaim, out long userId);

            if (!string.IsNullOrEmpty(userIdClaim))
            {
                var user = await _getUserNameByIdService.GetUserNameByIdAsync(userId);
                if (user != null && !string.IsNullOrEmpty(user.Name))
                {
                    userName = user.Name;
                }
            }
            return View("Default", userName);
        }

    }

}
