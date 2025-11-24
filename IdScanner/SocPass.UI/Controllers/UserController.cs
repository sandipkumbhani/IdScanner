using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
using System.Security.Claims;
namespace SocPass.Controllers
{
    [AuthorizeToken("Admin","Society")]
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
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            //IList<User> userList = new List<User>();
            IList<User> userList = await _userServices.GetUsersAsync();
            ViewBag.UserList = userList;
            return View("~/Views/User/UserList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddUser(int? id)
        {
            bool isAdmin = User.IsInRole("Admin");
            bool isSociety = User.IsInRole("Society");
            string title = id.HasValue ? "Edit" : "Add";

            ViewBag.CurrentRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var societies = await _Societyservices.GetSocietyAsync();
            var assignedSociety = isAdmin ? null : societies.FirstOrDefault();

            ViewBag.IsSocietyReadonly = !isAdmin;
            ViewBag.Societies = societies;

            var user = id.HasValue
                        ? await _userServices.GetUserByIdAsync(id.Value)
                        : new User();

            if (!isAdmin && assignedSociety != null && user.SocietyId == 0)
            {
                user.SocietyId = assignedSociety.SocietyId;
            }
            ViewBag.SelectedSocietyId = user.SocietyId > 0
                                            ? user.SocietyId
                                            : assignedSociety?.SocietyId;

            ViewBag.AssignedSocietyId = assignedSociety?.SocietyId;
            ViewBag.Title = title;

            await InitViewBag(); 

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User modelUsers, string action, int? flatId = null)
        {
            int selectedSociety = modelUsers.SocietyId ?? 0;
            string selectedBlock = Request.Form["BlockId"];
            string selectedFlat = Request.Form["FlatId"];
            void SetSelectedDropdowns()
            {
                ViewBag.SelectedSocietyId = selectedSociety;
                ViewBag.SelectedBlockId = selectedBlock;
                ViewBag.SelectedFlatId = selectedFlat;
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
                    await _userServices.UpdateUserAsync(modelUsers, flatId);
                    TempData["SuccessMessage"] = "User updated successfully.";
                }
                return RedirectToAction("UserList");
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                await InitViewBag();
                SetSelectedDropdowns();
                return View(modelUsers);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Something went wrong. Please try again.";
                await InitViewBag();
                SetSelectedDropdowns();
                return View(modelUsers);
            }
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
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var societies = await _Societyservices.GetSocietyAsync();
            ViewBag.SocietyList = societies;
            ViewBag.Societies = societies;
            IList<UserRole> userRoles = await _userServices.GetAllUserRoleAsync();
            ViewBag.RoleList = userRoles;
        }
    }
}