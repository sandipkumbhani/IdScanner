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
            IEnumerable<Event> eventList = await _eventService.GetEventAsync();
            ViewBag.EventsList = eventList.ToList();
            return View("~/Views/Event/EventList.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> AddEvent(int eventId)
        {
            try
            {
                bool isAdmin = User.IsInRole("Admin");

                var societies = await _societyService.GetSocietyAsync();
                var model = new Event();
                string title;

                if (eventId == 0)
                {
                    title = "Add";

                    if (!isAdmin)
                    {
                        var assignedSociety = societies.FirstOrDefault();
                        if (assignedSociety != null)
                            model.SocietyId = assignedSociety.SocietyId;
                    }
                }
                else
                {
                    title = "Edit";
                    model = await _eventService.GetEventById(eventId) ?? new Event();
                }
                ViewBag.Title = title;
                ViewBag.IsSocietyReadonly = !isAdmin;
                ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", model.SocietyId);

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred while loading event details: {ex.Message}";
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
