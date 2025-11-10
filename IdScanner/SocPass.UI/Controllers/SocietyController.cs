using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
using System.Security.Claims;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken]
    public class SocietyController : Controller
    {
        private readonly ISocietyService _societyService;
        private GlobalClass _globalClass;
        public SocietyController(ISocietyService societyService, GlobalClass globalClass)
        {
            _societyService = societyService
                ?? throw new ArgumentNullException(nameof(societyService));
            _globalClass = globalClass;
        }
        public async Task<IActionResult> SocietyList()
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IList<Society> SocietyList = await _societyService.GetAllSocietyAsync(userId);
            ViewBag.SocietyList = SocietyList;
            return View("~/Views/Society/SocietyList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddSociety(int? societyId)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            Society entity = new Society();
            string Title = "Add";
            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            if (societyId != null)
            {
                Title = "Edit";
                entity = await _societyService.GetSocietyByIdAsync(societyId.Value);
            }
            ViewBag.Title = Title;
            return View(entity);
        }
        [HttpPost]
        public async Task<IActionResult> AddSociety(Society society)
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
            if (society.SocietyId == 0)
            {
                await _societyService.AddSocietyAsync(society);

            }
            else
            {
                await _societyService.UpdateSocietyAsync(society);
            }
            return RedirectToAction("SocietyList");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteSociety(int societyId)
        {
            try
            {
                await _societyService.DeleteSocietyAsync(societyId);
                return RedirectToAction("SocietyList");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Society with ID {societyId} not found: {ex.Message}";
                return View("Error");
            }
        }
    }
}
