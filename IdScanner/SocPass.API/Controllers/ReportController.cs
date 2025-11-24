using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;

namespace SocPass.API.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet("GetReport")]
        public async Task<IActionResult> GetReport(int blockId, int eventId, DateTime startDate)
        {
            var data = await _reportService.GetReportAsync(blockId, eventId, startDate);
            return Json(data);
        }

    }
}
