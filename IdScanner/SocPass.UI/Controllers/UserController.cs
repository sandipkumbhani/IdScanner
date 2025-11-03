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

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
                return RedirectToAction("Login", "Login");

            // Extract current user context
            var currentUserRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int currentUserId);

            IList<User> userList = new List<User>();

            // 🔹 Admin – full visibility
            if (User.IsInRole("Admin"))
            {
                userList = await _userServices.GetAllUsersAsync();
            }
            else
            {
                // 🔹 Society User – limited visibility
                var societies = await _Societyservices.GetAllSocietyAsync(currentUserId);
                var assignedSociety = societies.FirstOrDefault();

                if (assignedSociety != null)
                {
                    // Filter user list by assigned society
                    userList = (await _userServices.GetAllUsersAsync())
                                .Where(u => u.SocietyId == assignedSociety.SocietyId && u.UserRole.Name != "Society")
                                .ToList();
                }
                else
                {
                    // Fallback – no associated society, return empty set
                    userList = new List<User>();
                }

            }

            ViewBag.UserList = userList;

            return View("~/Views/User/UserList.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> AddUser(int? id)
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
                return RedirectToAction("Login", "Login");

            ViewBag.CurrentRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;
            Society assignedSociety = null;

            // --- Role-based data isolation ---
            if (User.IsInRole("Admin"))
            {
                societies = await _Societyservices.GetAllSocietyAsync();
                ViewBag.IsSocietyReadonly = false;
            }
            else
            {
                societies = await _Societyservices.GetAllSocietyAsync(userId);
                assignedSociety = societies.FirstOrDefault();

                ViewBag.IsSocietyReadonly = true;
            }

            ViewBag.Societies = societies;
            await InitViewBag();

            // --- Initialize user model ---
            User user = id == null ? new User() : await _userServices.GetUserByIdAsync(id.Value);

            // --- Auto-assign society if not explicitly set ---
            if (!User.IsInRole("Admin") && assignedSociety != null && user.SocietyId == 0)
                user.SocietyId = assignedSociety.SocietyId;

            // --- Safe prefetch for block and flat ---
            int? selectedBlockId = null;
            int? selectedFlatId = null;

            
            if ((User.IsInRole("Society") || User.IsInRole("Admin")) && user.SocietyId > 0)
            {
                var blocks = await _blockService.GetBlockBySocietyId(user.SocietyId);
                var firstBlock = blocks.FirstOrDefault();
                if (firstBlock != null)
                {
                    selectedBlockId = firstBlock.BlockId;

                    var flats = await _flatService.GetFlatByBlockId(firstBlock.BlockId);
                    var firstFlat = flats.FirstOrDefault();
                    if (firstFlat != null)
                        selectedFlatId = firstFlat.FlatId;
                }
            }

            // --- Always provide prefilled ViewBag data ---
            ViewBag.SelectedSocietyId = user.SocietyId > 0 ? user.SocietyId : assignedSociety?.SocietyId;
            //ViewBag.SelectedBlockId = selectedBlockId;
            //ViewBag.SelectedFlatId = selectedFlatId;
            ViewBag.AssignedSocietyId = assignedSociety?.SocietyId;

            return View(user);
        }



        [HttpPost]
        public async Task<IActionResult> AddUser(User modelUsers, string action, int? flatId = null)
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }

            int selectedSociety =(int)modelUsers.SocietyId;
            string selectedBlock = Request.Form["BlockId"];
            string selectedFlat = Request.Form["FlatId"];

            // 🔹 Local function to restore dropdowns
            void SetSelectedDropdowns()
            {
                ViewBag.SelectedSocietyId = selectedSociety;
                ViewBag.SelectedBlockId = selectedBlock;
                ViewBag.SelectedFlatId = selectedFlat;
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
                    await _userServices.UpdateUserAsync(modelUsers,flatId);
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
            ViewBag.Societies = societies;
            IList<UserRole> userRoles = await _userServices.GetAllUserRoleAsync();
            ViewBag.RoleList = userRoles;
        }
    }
}
