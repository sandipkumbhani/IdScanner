using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
using System.Security.Claims;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken]
    public class MenuMasterController : Controller
    {
        private readonly IMenuMasterService _menuMasterService;
        public MenuMasterController(IMenuMasterService menuMasterService)
        {
            _menuMasterService = menuMasterService
                ?? throw new ArgumentNullException(nameof(menuMasterService));
        }
        public async Task<IActionResult> MenuMasterList()
        {
            IList<MenuMaster> MenuMasterList = await _menuMasterService.GetAllMenuMasterAsync();
            ViewBag.MenuMasterList = MenuMasterList;
            return View("~/Views/MenuMaster/MenuMasterList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddMenuMaster(int? id)
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            string Title;
            if (id == null)
            {
                Title = "Add";
                ViewBag.Title = Title;
                return View(new MenuMaster());
            }
            Title = "Edit";
            var user = await _menuMasterService.GetMenuByIdAsync(id.Value);
            ViewBag.Title = Title;
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> AddMenuMAster(MenuMaster menuMaster)
        {
            string NameMsg = string.Empty;
            if (string.IsNullOrEmpty(menuMaster.Name))
            {
                NameMsg = "Please Enter Name.";
                ViewBag.NameMsg = NameMsg;
            }

            string DescriptionMsg = string.Empty;
            if (string.IsNullOrEmpty(menuMaster.Description))
            {
                DescriptionMsg = "Please Enter Description.";
                ViewBag.DescriptionMsg = DescriptionMsg;
            }
            string UrlMsg = string.Empty;
            if (string.IsNullOrEmpty(menuMaster.Url))
            {
                UrlMsg = "Please Provide a Valid URL.";
                ViewBag.UrlMsg = UrlMsg;
            }
            string IconMsg = string.Empty;
            if (string.IsNullOrEmpty(menuMaster.Icon))
            {
                IconMsg = "Please Enter Icon Class.";
                ViewBag.IconMsg = IconMsg;
            }

            if (ViewBag.NameMsg != null || ViewBag.DescriptionMsg != null || ViewBag.UrlMsg != null || ViewBag.IconMsg != null)
            {
                return View(menuMaster);
            }
            if (menuMaster.MenuId == 0)
            {
                await _menuMasterService.AddMenuAsync(menuMaster);
            }
            else
            {
                await _menuMasterService.UpdateMenuAsync(menuMaster);
            }
            return RedirectToAction("MenuMasterList");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMenuMaster(int id)
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            try
            {
                await _menuMasterService.DeleteMenuAsync(id);
                return RedirectToAction("MenuMasterList");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"User with ID {id} not found: {ex.Message}";
                return View("Error");
            }
        }
    }
}
