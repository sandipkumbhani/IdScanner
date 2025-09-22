using Microsoft.AspNetCore.Mvc;

namespace IdScanner.UI.Controllers
{
    public class MemberDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
