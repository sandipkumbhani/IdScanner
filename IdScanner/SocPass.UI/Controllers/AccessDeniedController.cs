using Microsoft.AspNetCore.Mvc;

namespace SocPass.UI.Controllers
{
    public class AccessDeniedController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View("~/Views/AccessDenied/AccessDenied.cshtml");
        }

    }
}
