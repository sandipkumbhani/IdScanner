using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using System.Security.Claims;
using SocPass.UI.Filters;
namespace SocPass.UI.Controllers
{
    [AuthorizeToken("Admin", "Society")]
    public class ReportController : Controller
    {
        private readonly ISocietyService _societyService;
        private readonly IBlockService _blockService;
        private readonly IFlatService _flatService;
        private readonly IMemberService _memberService;
        private readonly IEventService _eventService;
        private readonly IReportService _reportService;
        public ReportController(ISocietyService societyService, IBlockService blockService, IFlatService flatService,
            IMemberService memberService, IEventService eventService, IReportService reportService)
        {
            _societyService = societyService;
            _blockService = blockService;
            _flatService = flatService;
            _memberService = memberService;
            _eventService = eventService;
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> MemberReport()
        { 
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            IEnumerable<Event> EventList;
            int selectedSocietyId = 0;
            IEnumerable<Society> societies = await _societyService.GetAllSocietyAsync();
            if (User.IsInRole("Admin"))
            {
                ViewBag.IsSocietyReadonly = false;
                selectedSocietyId = 0;
                EventList = await _eventService.GetAllEventAsync();
            }
            else
            {
                ViewBag.IsSocietyReadonly = true;
                selectedSocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
                EventList = await _eventService.GetEventBySocietyId(selectedSocietyId);
            }

            ViewBag.Societies = societies;
            ViewBag.EventList = EventList;
            ViewBag.SelectedSocietyId = selectedSocietyId;

            return View("/Views/Report/ReportDataList.cshtml");
        }
        [HttpGet]
        public async Task<JsonResult> GetReport(int blockId, int eventId, DateTime startDate)
        {
            var data = await _reportService.GetReportAsync(blockId, eventId, startDate);
            return new JsonResult(data);
        }

    }
}
