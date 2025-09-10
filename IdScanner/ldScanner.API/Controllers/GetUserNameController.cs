using IdScanner.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ldScanner.API.Controllers
{
    [Route("api/[controller]")]
    public class GetUserNameController : Controller
    {
        private readonly IGetLoginUserNameService _getLoginUserNameService;
        public GetUserNameController(IGetLoginUserNameService getLoginUserNameService)
        {
            _getLoginUserNameService = getLoginUserNameService ?? throw new ArgumentNullException(nameof(getLoginUserNameService));
        }
        [HttpGet("get-user-name")]
        public async Task<IActionResult> GetUserName(long userId)
        {
            var user = await _getLoginUserNameService.GetLoginUserNameAsync(userId);

            if (user == null || string.IsNullOrEmpty(user.Name))
                return NotFound(new { message = "User not found." });

            return Ok(new { name = user.Name });
        }

    }
}
