using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using System.Security.Claims;
using SocPass.UI.Filters;
namespace SocPass.UI.Controllers
{
    [AuthorizeToken("Admin")]
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
            IList<Society> SocietyList = await _societyService.GetSocietyAsync();
            ViewBag.SocietyList = SocietyList;
            return View("~/Views/Society/SocietyList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddSociety(int? societyId)
        {
            Society entity = new Society();
            string Title = "Add";
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
