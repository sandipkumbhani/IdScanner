using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocPass.Application.Interface;
using SocPass.Domain.Model;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Society")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [HttpGet("GetEventById")]
        public async Task<IActionResult> GetById(int eventId)
        {
            try
            {
                var result = await _eventService.GetEventByIdAsync(eventId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("getAllEvents")]
        public async Task<IActionResult> GetAllEvennt()
        {
            var getEvent = await _eventService.GetAllEventAsync();
            return Ok(getEvent);
        }
        [AllowAnonymous]
        [HttpGet("getEventBySocietyId")]
        public async Task<IActionResult> getEventBySocietyId(int SocietyId)
        {
            var getEvent = await _eventService.GetEventBySocietyAsync(SocietyId);
            return Ok(getEvent);
        }
        [AllowAnonymous]
        [HttpGet("getEventByUserId")]
        public async Task<IActionResult> GetEventByUserId(int userid)
        {
            var getEvent = await _eventService.GetEventByUserId(userid);
            return Ok(getEvent);
        }
        [HttpPost("Add-Event")]
        public async Task<IActionResult> AddEvent([FromBody] Event events)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addEvent = await _eventService.AddEventAsync(events);
            return Ok(addEvent);
        }
        [HttpDelete("Delete-Event")]
        public async Task<IActionResult> DeleteEventAsync(int eventId)
        {
            try
            {
                await _eventService.DeleteEventAsync(eventId);
                return Ok($"Event with ID {eventId} has been deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"Event with ID {eventId} not found: {ex.Message}");
            }
        }
        [HttpPut("Update-Event")]
        public async Task<IActionResult> UpdateMenuAsync([FromBody] Event events)
        {

            try
            {
                var updatedEvent = await _eventService.UpdateEventAsync(events);
                return Ok(updatedEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

}
