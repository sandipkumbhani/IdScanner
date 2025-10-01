using Emertec.UI.Application.Interface;
using Emertec.UI.Application.Services;
using Emertec.UI.Domain.Model;
using MicroService_Template.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace EmertecUI.Controllers
{
    public class UserController : Controller
    {
        IUserServices _userServices;
        IMenuMappingServices _menuMappingServices;
        IMenuMasterServices _menuMasterServices;
        private GlobalClass _globalClass;
        public UserController(IUserServices userServices, IMenuMappingServices menuMappingServices,IMenuMasterServices menuMasterServices, GlobalClass globalClass)
        {
            _userServices = userServices;
            _menuMappingServices = menuMappingServices;
            _menuMasterServices = menuMasterServices;
            _globalClass = globalClass;
        }
        public async Task<IActionResult> UserList()
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
            var role = HttpContext.User?.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            long.TryParse(userIdClaim, out long userId);
            IList<ModelUsers> userList = await _userServices.GetAllUsersAsync();
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                userList = userList.Where(u => u.UserId != userId).ToList();
            }
            else
            {
                userList = userList.Where(u => u.UserId == userId).ToList();
            }
            ViewBag.UserList = userList;
            return View("~/Views/User/UserList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddUser(int? id)
        {
            await InitViewBag();
            if (id == null)
            {
                return View(new ModelUsers());
            }
            var user = await _userServices.GetUserByIdAsync(id.Value);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(ModelUsers modelUsers,string action)
        {
            string NameMsg = string.Empty;
            if (string.IsNullOrEmpty(modelUsers.Name))
            {
                NameMsg = "Please Enter Name.";
                ViewBag.NameMsg = NameMsg;
            }
            string EmailMsg = string.Empty;
            if (string.IsNullOrEmpty(modelUsers.EmailId))
            {
                EmailMsg = "Please Enter EmailId.";
                ViewBag.EmailMsg = EmailMsg;
            }
            string passwordMsg = string.Empty;
            if (string.IsNullOrEmpty(modelUsers.Password))
            {
                passwordMsg = "Please Enter Password.";
                ViewBag.passwordMsg = passwordMsg;
            }
            string ConfirmPasswordMsg = string.Empty;
            if (modelUsers.Password != modelUsers.ConfirmPassword)
            {
                ConfirmPasswordMsg = "Password and Confirm Password do not match.";
                ViewBag.ConfirmPasswordMsg = ConfirmPasswordMsg;
            }
            string UserRoleMsg = string.Empty;
            if (modelUsers.UserRoleId == 0)
            {
                UserRoleMsg = "Please select a role.";
                ViewBag.UserRoleMsg = UserRoleMsg;
            }
            if (ViewBag.NameMsg != null || ViewBag.EmailMsg != null || ViewBag.passwordMsg != null || ViewBag.UserRoleMsg != null)
            {
                await InitViewBag();
                return View(modelUsers);
            }
            if (!ModelState.IsValid)
            {
                ViewBag.RoleList = await _userServices.GetAllUserRoleAsync();
                return View(modelUsers);
            }
            if (modelUsers.UserId == 0)
            {
                ModelUsers createdUser = await _userServices.AddUserAsync(modelUsers);
                long newUserId = createdUser.UserId; 
                var roles = await _userServices.GetRoleNameByIdAsync(modelUsers.UserRoleId);
                string roleName = roles.Name ?? "";
                if (string.Equals(roleName, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    var allMenus = await _menuMasterServices.GetAllMenuMasterAsync();
                    var menuMapping = new ModelUserMenuMapping
                    {
                        UserId = createdUser.UserId,
                        MenuIds = string.Join(",", allMenus.Select(m => m.MenuId)),
                    };
                    await _menuMappingServices.AddMenuMappingAsync(menuMapping);
                }
            }
            else
            {
                await _userServices.UpdateUserAsync(modelUsers);
            }
            return RedirectToAction("UserList");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userServices.Deleteuserasync(id);
                return RedirectToAction("UserList");
            }
            catch (KeyNotFoundException ex)
            {
                ViewBag.ErrorMessage = $"User with ID {id} not found: {ex.Message}";
                return View("Error");
            }
        }
        private async Task InitViewBag()
        {
            IList<ModelUserRole> userRoles = await _userServices.GetAllUserRoleAsync();
            ViewBag.RoleList = userRoles;
        }
    }
}
