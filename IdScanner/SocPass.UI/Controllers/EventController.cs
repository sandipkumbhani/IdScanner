using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using System.Security.Claims;
using SocPass.UI.Filters;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken("Admin", "Society")]
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
            IEnumerable<Event> eventList = await _eventService.GetAllEventAsync();
            ViewBag.EventsList = eventList.ToList();
            return View("~/Views/Event/EventList.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> AddEvent(int eventId)
        {
            try
            {
                var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
                int.TryParse(userIdClaim, out int userId);
                string Title;
                IEnumerable<Society> societies;
                societies = await _societyService.GetAllSocietyAsync();
                if (eventId == 0)
                {
                    Title = "Add";
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
                    ViewBag.Title = Title;
                    return View(newEvent);
                }
                Title = "Edit";
                var existingEvent = await _eventService.GetEventById(eventId);
                var eventModel = existingEvent ?? new Event();

                ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", eventModel.SocietyId);
                ViewBag.IsSocietyReadonly = !User.IsInRole("Admin");
                ViewBag.Title = Title;

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
