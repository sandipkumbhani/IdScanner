using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using System.Security.Claims;
using SocPass.UI.Filters;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken("Admin", "Society")]
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
            bool isAdmin = User.IsInRole("Admin");

            var societies = await _societyService.GetSocietyAsync();
            var events = await _eventService.GetEventAsync();

            int selectedSocietyId = 0;

            if (!isAdmin)
            {
                selectedSocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            }
            ViewBag.IsSocietyReadonly = !isAdmin;
            ViewBag.Societies = societies;
            ViewBag.EventList = events;
            ViewBag.SelectedSocietyId = selectedSocietyId;

            return View("/Views/MembersQrList/MembersQrList.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateMemberPass(int societyId, int blockId, int EventId)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            ViewBag.SelectedSocietyId = societyId;
            await _memberService.GenerateMemberPass(blockId, EventId);
            var qrList = await _flatRepository.GetQR(blockId, EventId);
            return View("/Views/MembersQrList/QrList.cshtml", qrList);
        }
        [HttpGet]
        public async Task<IActionResult> GuestQrList()
        {
            bool isAdmin = User.IsInRole("Admin");
            var societies = await _societyService.GetSocietyAsync();
            var events = await _eventService.GetEventAsync();
            int selectedSocietyId = 0;

            if (!isAdmin)
            {
                selectedSocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            }
            ViewBag.IsSocietyReadonly = !isAdmin;
            ViewBag.Societies = societies;
            ViewBag.EventList = events;
            ViewBag.SelectedSocietyId = selectedSocietyId;

            return View("/Views/GuestQrList/GuestQrList.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateGuestPass(int societyId, int blockId, int EventId)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            ViewBag.SelectedSocietyId = societyId;
            await _memberService.GenerateGuestPass(blockId, EventId);
            var QRlist = await _flatRepository.GetGuestQR(blockId, EventId);

            return View("/Views/GuestQrList/GuestQr.cshtml", QRlist);
        }
    }
}
