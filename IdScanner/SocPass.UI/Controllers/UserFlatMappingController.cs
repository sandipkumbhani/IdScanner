using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Controllers
{
    public class UserFlatMappingController : Controller
    {
        private readonly IUserFlatMappingService _userFlatMappingService;
        private readonly GlobalClass _globalClass;
        public UserFlatMappingController(IUserFlatMappingService userFlatMappingService,GlobalClass globalClass)
        {
            _userFlatMappingService = userFlatMappingService;
            _globalClass = globalClass;
        }
        public async Task<IActionResult> GetQrByUserId()
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var qrList = await _userFlatMappingService.GetQrByUserId(userId);
            ViewBag.qrList = qrList;
            return View("~/Views/UserQR/UserQR.cshtml");
        }
    }
}
