using Microsoft.AspNetCore.Mvc;

namespace SocPass.API.Controllers
{
    public class UserRoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
