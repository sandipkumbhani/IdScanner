using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Filters;
using System.Security.Claims;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ISocietyService _societyService;
        public EventController(IEventService eventService, ISocietyService societyService)
        {
            _eventService = eventService;
            _societyService = societyService;

        }
        [HttpGet]
        public async Task<IActionResult> EventList()
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }

            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Event> eventList;

            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                eventList = await _eventService.GetAllEventAsync();
            }
            else
            {
                var societies = await _societyService.GetAllSocietyAsync(userId);
                var society = societies.FirstOrDefault();

                if (society != null)
                {
                    eventList = (await _eventService.GetAllEventAsync())
                                .Where(e => e.SocietyId == society.SocietyId)
                                .ToList();
                }
                else
                {
                    eventList = new List<Event>();
                }
            }

            ViewBag.EventsList = eventList.ToList();
            //ViewBag.IsAdmin = role == "Admin";

            //ViewBag.UserSocietyName = (await _societyService.GetAllSocietyAsync(userId))
            //    .FirstOrDefault()?.Name ?? "";

            return View("~/Views/Event/EventList.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> AddEvent(int eventId)
        {
            try
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

                if (eventId == 0)
                {
                    var newEvent = new Event();

                    if (!User.IsInRole("Admin"))
                    {
                        var assignedSociety = societies.FirstOrDefault();
                        if (assignedSociety != null)
                            newEvent.SocietyId = assignedSociety.SocietyId;

                        ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", newEvent.SocietyId);
                        ViewBag.IsSocietyReadonly = true;
                    }
                    else
                    {
                        ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name");
                        ViewBag.IsSocietyReadonly = false;
                    }

                    return View(newEvent);
                }

                var existingEvent = await _eventService.GetEventById(eventId);
                var eventModel = existingEvent ?? new Event();

                ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", eventModel.SocietyId);
                ViewBag.IsSocietyReadonly = !User.IsInRole("Admin");

                return View(eventModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading event details: " + ex.Message;
                return View(new Event());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent(Event events)
        {
           
            if (events.EventId == 0)
            {
                await _eventService.AddEventAsync(events);
            }
            else
            {
                await _eventService.UpdateEventAsync(events);
            }
            return RedirectToAction("EventList");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            try
            {
                await _eventService.DeleteEventAsync(eventId);
                return RedirectToAction("EventList");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Event with ID {eventId} not found: {ex.Message}";
                return View("Error");
            }
        }
    }
}
