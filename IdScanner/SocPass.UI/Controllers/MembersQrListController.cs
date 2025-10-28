using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
using System.Security.Claims;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken]
    public class MembersQrListController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IBlockService _blockService;
        private readonly ISocietyService _societyService;
        private readonly IFlatService _flatRepository;
        private readonly GlobalClass _globalClass;
        public MembersQrListController(IMemberService memberService, IBlockService blockService, ISocietyService societyService, IFlatService flatService, GlobalClass globalClass)
        {
            _memberService = memberService;
            _blockService = blockService;
            _societyService = societyService;
            _flatRepository = flatService;
            _globalClass = globalClass;

        }

        [HttpGet]
        public async Task<IActionResult> MemberQrList()
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;
            int selectedSocietyId = 0;
            if (User.IsInRole("Admin"))
            {
                // Admin sees all societies
                societies = await _societyService.GetAllSocietyAsync();
                ViewBag.IsSocietyReadonly = false;
                selectedSocietyId = 0;
            }
            else
            {
                // User sees only assigned societies
                societies = await _societyService.GetAllSocietyAsync(userId);
                ViewBag.IsSocietyReadonly = true;
                selectedSocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            }

            ViewBag.Societies = societies;
            ViewBag.SelectedSocietyId = selectedSocietyId;

            return View("/Views/MembersQrList/MembersQrList.cshtml");

        }
        [HttpPost]
        public async Task<IActionResult> GeneratePass(int societyId, int blockId, DateTime passDate)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            ViewBag.SelectedSocietyId = societyId;

            // Validate inputs
            if (societyId <= 0 || blockId <= 0 || passDate == default)
            {
                ViewBag.Error = "Invalid input. Please fill all fields correctly.";

                // Repopulate societies for the dropdown
                IEnumerable<Society> societies;
                if (User.IsInRole("Admin"))
                {
                    societies = await _societyService.GetAllSocietyAsync();
                    ViewBag.IsSocietyReadonly = false;
                }
                else
                {
                    societies = await _societyService.GetAllSocietyAsync(userId);
                    ViewBag.IsSocietyReadonly = true;
                }

                ViewBag.Societies = societies;

                return View("/Views/MembersQrList/MembersQrList.cshtml");
            }

            // Generate QR codes and display result
            await _memberService.GeneratePass(blockId, passDate);
            var QRlist = await _flatRepository.GetQR(blockId);

            return View("/Views/MembersQrList/QrList.cshtml", QRlist);
        }

        [HttpGet]
        public async Task<IActionResult> GuestQrList()
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;
            int selectedSocietyId = 0;
            if (User.IsInRole("Admin"))
            {
                // Admin sees all societies
                societies = await _societyService.GetAllSocietyAsync();
                ViewBag.IsSocietyReadonly = false;
                selectedSocietyId = 0;
            }
            else
            {
                // User sees only assigned societies
                societies = await _societyService.GetAllSocietyAsync(userId);
                ViewBag.IsSocietyReadonly = true;
                selectedSocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            }

            ViewBag.Societies = societies;
            ViewBag.SelectedSocietyId = selectedSocietyId;
            return View("/Views/GuestQrList/GuestQrList.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> GenerateGuestPass(int societyId, int blockId, DateTime passDate)
        {
            if (blockId <= 0 || passDate == default(DateTime) /*|| passDate < DateTime.Today*/)
            {
                ViewBag.Error = "Invalid block or date";

                var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
                int.TryParse(userIdClaim, out int userId);

                IEnumerable<Society> societies;
                if (User.IsInRole("Admin"))
                {
                    societies = await _societyService.GetAllSocietyAsync();
                    ViewBag.IsSocietyReadonly = false;
                }
                else
                {
                    societies = await _societyService.GetAllSocietyAsync(userId);
                    ViewBag.IsSocietyReadonly = true;
                }

                ViewBag.Societies = societies;
                ViewBag.SelectedSocietyId = societyId;

                return View();
            }

            await _memberService.GenerateGuestPass(blockId, passDate);
            var QRlist = await _flatRepository.GetGuestQR(blockId);

            return View("/Views/GuestQrList/GuestQr.cshtml", QRlist);
        }


        [HttpGet]
        public async Task<JsonResult> GetBlocksBySociety(int societyId)
        {
            var blocks = await _blockService.GetBlockBySocietyId(societyId);
            var result = blocks.Select(b => new {
                blockId = b.BlockId,
                blockName = b.BlockNumber
            });
            return Json(result);
        }
    }
}
