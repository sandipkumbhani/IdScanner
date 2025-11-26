using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using System.Security.Claims;
using SocPass.UI.Filters;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken("Admin")]
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
            string OrderMsg = string.Empty;
            if (menuMaster.MenuOrder <= 0)
            {
                OrderMsg = "Please Enter Menu Order.";
                ViewBag.OrderMsg = OrderMsg;
            }

            if (ViewBag.NameMsg != null || ViewBag.DescriptionMsg != null || ViewBag.UrlMsg != null || ViewBag.IconMsg != null || ViewBag.OrderMsg != null)
            {
                return View(menuMaster);
            }
            try
            {
                string message;

                if (menuMaster.MenuId == 0)
                {
                    message = await _menuMasterService.AddMenuAsync(menuMaster);
                }
                else
                {
                    message = await _menuMasterService.UpdateMenuAsync(menuMaster);
                }

                if (message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                {
                    ViewBag.ErrorMessage = message;
                    return View(menuMaster);
                }

                TempData["SuccessMessage"] = "Menu saved successfully!";
                return RedirectToAction("MenuMasterList");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(menuMaster);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMenuMaster(int id)
        {
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
