using Microsoft.AspNetCore.Http;
using SocPass.Application.Interface;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System.Security.Claims;

namespace SocPass.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ISocietyRepository _societyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EventService(IEventRepository eventRepository, ISocietyRepository societyRepository, IHttpContextAccessor httpContextAccessor)
        {
            _eventRepository = eventRepository;
            _societyRepository = societyRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Event> AddEventAsync(Event events)
        {
            var addEvent = new Event
            {
                SocietyId = events.SocietyId,
                EventName = events.EventName,
                Description = events.Description,
                StartDate = events.StartDate,
                EndDate = events.EndDate,
                Location = events.Location,
                Organizer = events.Organizer,
                Society = events.Society,
                StartTime = events.StartTime,
                EndTime = events.EndTime,
                IsActive = true,
                InsertBy = 1,
                InsertDate = DateTime.Now,
                UpdateBy = 1,
                UpdateDate = DateTime.Now,
            };
            return await _eventRepository.AddEventAsync(events);
        }
        public async Task<List<Event>> GetEventAsync()
        {
            string role = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var eventList = new List<Event>();
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                 eventList = await _eventRepository.GetEventsAsync();
            }
            else
            {
                var societies = await _societyRepository.GetSocietyAsync(userId);
                var society = societies.FirstOrDefault();

                if (society != null)
                {
                    eventList = await _eventRepository.GetEventListBySocietyAsync(society.SocietyId);

                }
                else
                {
                    eventList = new List<Event>();
                }
            }
            return eventList ?? new List<Event>();
        }
        public async Task<Event?> GetEventByIdAsync(int eventId)
        {
            var result = await _eventRepository.GetEventByIdAsync(eventId);
            if (result == null)
            {
                throw new KeyNotFoundException($"Event with Id {eventId} not found");
            }
            return result;  
        }
        public async Task<Event> UpdateEventAsync(Event events)
        {
            var existingEvent = await _eventRepository.GetEventByIdAsync(events.EventId);
            if (existingEvent == null)
            {
                throw new KeyNotFoundException($"Event with Id {events.EventId} not found");
            }
            existingEvent.SocietyId = events.SocietyId;
            existingEvent.EventName = events.EventName;
            existingEvent.Description = events.Description;
            existingEvent.StartDate = events.StartDate;
            existingEvent.EndDate = events.EndDate;
            existingEvent.Location = events.Location;
            existingEvent.Organizer = events.Organizer;
            existingEvent.UpdateBy = 1;
            existingEvent.UpdateDate = DateTime.Now;
            return await _eventRepository.UpdateEventAsync(existingEvent);
        }
        public async Task DeleteEventAsync(int eventId)
        {
            var existingEvent = await _eventRepository.GetEventByIdAsync(eventId);
            if (existingEvent == null)
            {
                throw new KeyNotFoundException($"Event with Id {eventId} not found");
            }
            await _eventRepository.DeleteEventAsync(eventId);
        }
        public async Task<List<Event>> GetEventBySocietyAsync(int SocietyId)
        {
            var eventList = await _eventRepository.GetEventListBySocietyAsync(SocietyId);
            return eventList ?? new List<Event>();
        }
        public async Task<List<Event>> GetEventByUserId(int userid)
        {
            var eventList = await _eventRepository.GetEventListByUserIdAsync(userid);
            return eventList ?? new List<Event>();

        }
    }
}
