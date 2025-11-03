using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
using System.Globalization;
using System.Security.Claims;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken]
    public class ReportController : Controller
    {
        private readonly ISocietyService _societyService;
        private readonly IBlockService _blockService;
        private readonly IFlatService _flatService;
        private readonly IMemberService _memberService;
        private readonly IEventService _eventService;
        public ReportController(ISocietyService societyService, IBlockService blockService, IFlatService flatService, IMemberService memberService, IEventService eventService
            )
        {
            _societyService = societyService;
            _blockService = blockService;
            _flatService = flatService;
            _memberService = memberService;
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> MemberReport()
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
                societies = await _societyService.GetAllSocietyAsync();
                ViewBag.IsSocietyReadonly = false;
                selectedSocietyId = 0;
            }
            else
            {
                societies = await _societyService.GetAllSocietyAsync(userId);
                ViewBag.IsSocietyReadonly = true;
                selectedSocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            }

            ViewBag.Societies = societies;
            ViewBag.SelectedSocietyId = selectedSocietyId;

            return View("/Views/Report/ReportDataList.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> GetBlocksBySociety(int societyId)
        {
            var blocks = await _blockService.GetBlockBySocietyId(societyId);
            return Json(blocks.Select(b => new { blockId = b.BlockId, blockName = b.BlockNumber }));
        }

        [HttpGet]
        public async Task<JsonResult> GetFlatsByBlock(int blockId, int eventId, string selectedDate)
        {
            var flatReports = new List<object>();
            if (blockId <= 0 || eventId <= 0 || string.IsNullOrEmpty(selectedDate))
                return Json(new object[0]);
            // Parse selected date
            if (!DateOnly.TryParseExact(selectedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var filterDate))
                return Json(new object[0]);
            // Get event details
            var selectedEvent = await _eventService.GetEventById(eventId);
            if (selectedEvent == null)
                return Json(new object[0]);
            var eventDate = DateOnly.FromDateTime(selectedEvent.StartDate);
            // Validate event date with selected date
           if (eventDate != filterDate)
                return Json(new object[0]); // no records if selected date doesn't match event start date
                                            // Get all flats under the selected block
            var flats = await _flatService.GetFlatByBlockId(blockId);
            foreach (var f in flats)
            {
                // Skip flats not in the same society as the event
                if (f.SocietyId != selectedEvent.SocietyId)
                    continue;
                // Get members under the flat
                var members = await _memberService.GetAllMember(f.FlatId);
                int visitedAdults = members.Count(m => m.Visited && !m.IsChild);
                int visitedChildren = members.Count(m => m.Visited && m.IsChild);
                int pending = f.TotalMember - (visitedAdults + visitedChildren);
                flatReports.Add(new
                {
                    f.FlatId,
                    f.FlatNumber,
                    f.NumberOfAdult,
                    f.NumberOfChild,
                    f.TotalMember,
                    selectedEvent.EventId,
                    selectedEvent.EventName,
                    EventDate = eventDate,
                    VisitedAdults = visitedAdults,
                    VisitedChildren = visitedChildren,
                    Pending = pending
                });
            }
            return Json(flatReports);
        }

    }
}
