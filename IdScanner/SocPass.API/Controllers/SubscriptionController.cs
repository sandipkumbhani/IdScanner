using Microsoft.AspNetCore.Mvc;

namespace SocPass.API.Controllers
{
    public class SubscriptionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
