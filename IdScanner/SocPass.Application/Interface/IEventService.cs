using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Interface
{
    public interface IEventService
    {
        Task<Event> AddEventAsync(Event events);
        Task<Event?> GetEventByIdAsync(int eventId);
        Task<List<Event>> GetAllEventAsync();
        Task DeleteEventAsync(int eventId);
        Task<Event> UpdateEventAsync(Event events);
        Task<List<Event>> GetEventBySocietyAsync(int SocietyId);
        Task<List<Event>> GetEventByUserId(int userid);
    }
}
