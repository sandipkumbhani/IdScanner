using SocPass.Application.Interface;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
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
        public async Task<List<Event>> GetAllEventAsync()
        {
            var eventList = await _eventRepository.GetAllEventsAsync();
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
            await _eventRepository.DeleteEventAsync(existingEvent);
        }
        public async Task<List<Event>> GetEventBySocietyAsync(int SocietyId)
        {
            var eventList = await _eventRepository.GetEventListBySocietyAsync(SocietyId);
            return eventList ?? new List<Event>();
        }
    }
}
