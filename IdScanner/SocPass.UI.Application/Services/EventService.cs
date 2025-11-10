using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventAdapter _eventRepository;
        public EventService(IEventAdapter eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task<IList<Event>> GetAllEventAsync()
        {
            return await _eventRepository.GetAllEventAsync();
        }
        public async Task<string> AddEventAsync(Event events)
        {
            return await _eventRepository.AddEventAsync(events);
        }
        public async Task<Event?> GetEventById(int eventId)
        {
            return await _eventRepository.GetEventByIdAsync(eventId);
        }
        public async Task<IList<Event>> GetEventBySocietyId(int SocietyId)
        {
            return await _eventRepository.GetEventBySocietyId(SocietyId);
        }
        public async Task<IList<Event>> GetEventByUserId(int userid)
        {
            return await _eventRepository.GetEventByUserId(userid);
        }
        public async Task<string?> UpdateEventAsync(Event events)
        {
            return await _eventRepository.UpdateEventAsync(events);
        }
        public async Task<string> DeleteEventAsync(int eventId)
        {
            return await _eventRepository.DeleteMenuAsync(eventId);
        }

    }
}
