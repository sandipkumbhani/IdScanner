using Microsoft.AspNetCore.Mvc;

namespace SocPass.API.Controllers
{
    public class BlockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
