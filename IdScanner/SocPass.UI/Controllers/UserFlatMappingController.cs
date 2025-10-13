using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;

namespace SocPass.UI.Controllers
{
    public class UserFlatMappingController : Controller
    {
        private readonly IUserFlatMappingService _userFlatMappingService;
        public UserFlatMappingController(IUserFlatMappingService userFlatMappingService)
        {
            _userFlatMappingService = userFlatMappingService;
        }
        public async Task<IActionResult> GetQrByUserId()
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var qrList = await _userFlatMappingService.GetQrByUserId(userId);
            ViewBag.qrList = qrList;
            return View("~/Views/UserQR/UserQR.cshtml");
        }
    }
}
