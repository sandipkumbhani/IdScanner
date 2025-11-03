using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
using System.Security.Claims;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken]
    public class UserFlatMappingController : Controller
    {
        private readonly IUserFlatMappingService _userFlatMappingService;
        private readonly IEventService _eventService;
        private readonly ISocietyService _societyService;
        public UserFlatMappingController(IUserFlatMappingService userFlatMappingService, IEventService eventService, ISocietyService societyService)
        {
            _userFlatMappingService = userFlatMappingService;
            _eventService = eventService;
            _societyService = societyService;
        }
        [HttpGet]
        public async Task<IActionResult> GetEventList()
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Event> eventList;
            if (User.IsInRole("Admin"))
                eventList = await _eventService.GetAllEventAsync();
            else
                eventList = await _eventService.GetEventByUserId(userId);
            ViewBag.EventList = eventList;
            return View("~/Views/UserQR/UserQR.cshtml");
        }
        [HttpGet("GetQrList")]
        public async Task<IActionResult> GetQrList(int? EventId)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var qrList = await _userFlatMappingService.GetQrByUserId(userId, EventId.Value);
            var result = qrList.Select(q => new
            {
                qrCodeUrl = q.QRCodeUrl,
                eventName = q.Event?.EventName ?? "N/A",
                isGuest = q.Member.IsGuest
            });

            return Json(result);
        }


    }
}
