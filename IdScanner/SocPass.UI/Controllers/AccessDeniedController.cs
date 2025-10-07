using Microsoft.AspNetCore.Mvc;

namespace SocPass.UI.Controllers
{
    public class AccessDeniedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
