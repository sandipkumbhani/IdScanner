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
        private readonly IEventService _eventService;
        private readonly IReportService _reportService;
        public ReportController(ISocietyService societyService, IBlockService blockService,IEventService eventService, IReportService reportService)
        {
            _societyService = societyService;
            _eventService = eventService;
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> MemberReport()
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
