using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
using System.Security.Claims;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken]
    public class FlatController : Controller
    {
        private readonly IFlatService _flatService;
        private readonly ISocietyService _societyService;
        private readonly IBlockService _blockService;
        private GlobalClass _globalClass;
        public FlatController(IFlatService flatService, GlobalClass globalClass, ISocietyService societyService, IBlockService blockService)
        {
            _flatService = flatService
                ?? throw new ArgumentNullException(nameof(flatService));

            _societyService = societyService
                ?? throw new ArgumentNullException(nameof(societyService));
            _globalClass = globalClass;
            _blockService = blockService;
        }
        public async Task<IActionResult> FlatList()
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            var flatList = (await _flatService.GetAllFlatAsync()).ToList();

            ICollection<Flat> FlatList;
            if(string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                FlatList = await _flatService.GetAllFlatAsync();
            }
            else
            {
                var societies = await _societyService.GetAllSocietyAsync(userId);
                var society = societies.FirstOrDefault();
                if (society != null)
                {
                    FlatList = flatList
                                .Where(e => e.SocietyId == society.SocietyId)
                                .ToList();
                }
                else
                {
                    FlatList = new List<Flat>();
                }
            }
            ViewBag.FlatList = FlatList.ToList();
            //ViewBag.IsAdmin = User.IsInRole("Admin");

            //// Pass user's society name if not admin
            //if (!User.IsInRole("Admin"))
            //{
            //    var societies = await _societyService.GetAllSocietyAsync(userId);
            //    var userSocietyName = societies.FirstOrDefault()?.Name ?? "";
            //    ViewBag.UserSocietyName = userSocietyName;
            //}
            //else
            //{
            //    ViewBag.UserSocietyName = "";
            //}

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddFlat(int? societyId, int blockId)
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;

            if (User.IsInRole("Admin"))
            {
                societies = await _societyService.GetAllSocietyAsync();
            }
            else
            {
                societies = await _societyService.GetAllSocietyAsync(userId);
            }
            Flat flat;

            if (societyId == null || blockId == 0)
            {
                flat = new Flat();

                if (!User.IsInRole("Admin"))
                {
                    var assignedSociety = societies.FirstOrDefault();
                    if (assignedSociety != null)
                        flat.SocietyId = assignedSociety.SocietyId;

                    ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", flat.SocietyId);
                    ViewBag.IsSocietyReadonly = true;
                }
                else
                {
                    ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name");
                    ViewBag.IsSocietyReadonly = false;
                }
                return View(flat);
            }

            var flatList = await _flatService.GetFlatByIdAsync(societyId, blockId);
            flat = flatList.FirstOrDefault() ?? new Flat();
            ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", flat.SocietyId);
            ViewBag.IsSocietyReadonly = !User.IsInRole("Admin");
            return View(flat);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlat(Flat flat)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;

            if (User.IsInRole("Admin"))
            {
                // Admin → all societies
                societies = await _societyService.GetAllSocietyAsync();
            }
            else
            {
                var assignedSociety = await _societyService.GetAllSocietyAsync(userId);
                societies = assignedSociety; 
                flat.SocietyId = societies.FirstOrDefault()?.SocietyId ?? 0; 
            }

            ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", flat.SocietyId);
            ViewBag.IsSocietyReadonly = !User.IsInRole("Admin");

            if (!ModelState.IsValid)
            {
                // Pass validation errors to view
                return View(flat);
            }

            try
            {
                await _flatService.UpdateFlatAsync(flat);
                TempData["SuccessMessage"] = "Flat updated successfully.";
                return RedirectToAction("FlatList");
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(flat);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An unexpected error occurred while saving the flat.";
                return View(flat);
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
    }
}
