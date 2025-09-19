using Microsoft.AspNetCore.Mvc;

namespace SocPass.UI.Controllers
{
    public class MembersQrListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
