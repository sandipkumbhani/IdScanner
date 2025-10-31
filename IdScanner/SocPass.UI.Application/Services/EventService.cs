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
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task<List<Event>> GetAllEventAsync()
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
        public async Task<List<Event>> GetEventBySocietyId(int SocietyId)
        {
            return await _eventRepository.GetEventBySocietyId(SocietyId);
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
