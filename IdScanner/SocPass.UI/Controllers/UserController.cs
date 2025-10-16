using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using System.Security.Claims;

namespace SocPass.Controllers
{
    public class UserController : Controller
    {
        IUserService _userServices;
        private GlobalClass _globalClass;
        private readonly ISocietyService _Societyservices;
        private readonly IBlockService _blockService;
        private readonly IFlatService _flatService;
        public UserController(IUserService userServices, GlobalClass globalClass, ISocietyService societyservices, IBlockService blockService, IFlatService flatService)
        {
            _userServices = userServices;
            _globalClass = globalClass;
            _Societyservices = societyservices;
            _blockService = blockService;
            _flatService = flatService;
        }
        public async Task<IActionResult> UserList()
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
            IList<User> userList = await _userServices.GetAllUsersAsync();
            ViewBag.UserList = userList;
            return View("~/Views/User/UserList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddUser(int? id)
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
            await InitViewBag();
            if (id == null)
            {
                return View(new User());
            }
            var user = await _userServices.GetUserByIdAsync(id.Value);
            return View(user);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddUser(User modelUsers, string action, int? flatId = null)
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
            if (string.IsNullOrEmpty(modelUsers.Name))
            {
                ViewBag.ErrorMessage = "Please enter name.";
                await InitViewBag();
                return View(modelUsers);
            }
            if (string.IsNullOrEmpty(modelUsers.EmailId))
            {
                ViewBag.ErrorMessage = "Please enter email.";
                await InitViewBag();
                return View(modelUsers);
            }
            if (string.IsNullOrEmpty(modelUsers.Password))
            {
                ViewBag.ErrorMessage = "Please enter password.";
                await InitViewBag();
                return View(modelUsers);
            }
            if (modelUsers.Password != modelUsers.ConfirmPassword)
            {
                ViewBag.ErrorMessage = "Password and confirm password do not match.";
                await InitViewBag();
                return View(modelUsers);
            }
            if (modelUsers.UserRoleId == 0)
            {
                ViewBag.ErrorMessage = "Please select a role.";
                await InitViewBag();
                return View(modelUsers);
            }
            try
            {
                if (modelUsers.UserId == 0)
                {
                    await _userServices.AddUserAsync(modelUsers, flatId);
                    TempData["SuccessMessage"] = "User added successfully.";
                }
                else
                {
                    await _userServices.UpdateUserAsync(modelUsers);
                    TempData["SuccessMessage"] = "User updated successfully.";
                }

                return RedirectToAction("UserList");
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                await InitViewBag();
                return View(modelUsers);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Something went wrong. Please try again.";
                await InitViewBag();
                return View(modelUsers);
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
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
        [HttpGet]
        public async Task<JsonResult> GetBlocksBySociety(int societyId)
        {
            var blocks = await _blockService.GetBlockBySocietyId(societyId);
            var result = blocks.Select(d => new
            {
                blockId = d.BlockId,
                blockNumber = d.BlockNumber
            });
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetFlatsByBlock(int blockId)
        {
            var flats = await _flatService.GetFlatByBlockId(blockId);
            var result = flats.Select(f => new
            {
                flatId = f.FlatId,
                flatNumber = f.FlatNumber
            });
            return Json(result);
        }
        private async Task InitViewBag()
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var societies = await _Societyservices.GetAllSocietyAsync(userId);
            ViewBag.SocietyList = societies;
            IList<UserRole> userRoles = await _userServices.GetAllUserRoleAsync();
            ViewBag.RoleList = userRoles;
        }
    }
}
