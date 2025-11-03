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
        private readonly IEventService _eventService;

        public MembersQrListController(IMemberService memberService, IBlockService blockService, ISocietyService societyService, IFlatService flatService, IEventService eventService)
        {
            _memberService = memberService;
            _blockService = blockService;
            _societyService = societyService;
            _flatRepository = flatService;
            _eventService = eventService;
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
            IEnumerable<Event> EventList;
            int selectedSocietyId = 0;
            if (User.IsInRole("Admin"))
            {
                societies = await _societyService.GetAllSocietyAsync();
                ViewBag.IsSocietyReadonly = false;
                selectedSocietyId = 0;
                EventList = await _eventService.GetAllEventAsync();
            }
            else
            {
                societies = await _societyService.GetAllSocietyAsync(userId);
                ViewBag.IsSocietyReadonly = true;
                selectedSocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
                EventList = await _eventService.GetEventBySocietyId(selectedSocietyId);
            }
            ViewBag.EventList = EventList;
            ViewBag.Societies = societies;
            ViewBag.SelectedSocietyId = selectedSocietyId;
            return View("/Views/MembersQrList/MembersQrList.cshtml");

        }
        [HttpPost]
        public async Task<IActionResult> GeneratePass(int societyId, int blockId, int EventId)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            ViewBag.SelectedSocietyId = societyId;

            if (societyId <= 0 || blockId <= 0 || EventId == default)
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
            await _memberService.GeneratePass(blockId, EventId);
            var qrList = await _flatRepository.GetQR(blockId, EventId);
            return View("/Views/MembersQrList/QrList.cshtml", qrList);


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
            IEnumerable<Event> EventList;
            int selectedSocietyId = 0;
            if (User.IsInRole("Admin"))
            {
                societies = await _societyService.GetAllSocietyAsync();
                ViewBag.IsSocietyReadonly = false;
                selectedSocietyId = 0;
                EventList = await _eventService.GetAllEventAsync();
            }
            else
            {
                societies = await _societyService.GetAllSocietyAsync(userId);
                ViewBag.IsSocietyReadonly = true;
                selectedSocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
                EventList = await _eventService.GetEventBySocietyId(selectedSocietyId);
            }
            ViewBag.EventList = EventList;
            ViewBag.Societies = societies;
            ViewBag.SelectedSocietyId = selectedSocietyId;
            return View("/Views/GuestQrList/GuestQrList.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> GenerateGuestPass(int societyId, int blockId, int EventId)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            ViewBag.SelectedSocietyId = societyId;

            if (societyId <= 0 || blockId <= 0 || EventId == default)
            {
                ViewBag.Error = "Invalid input. Please fill all fields correctly.";

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
                return View();
            }

            await _memberService.GenerateGuestPass(blockId, EventId);
            var QRlist = await _flatRepository.GetGuestQR(blockId, EventId);

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
