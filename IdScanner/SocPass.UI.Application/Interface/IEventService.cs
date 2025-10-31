using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Application.Interface
{
    public interface IEventService
    {
        Task<List<Event>> GetAllEventAsync();
        Task<Event?> GetEventById(int eventId);
        Task<string> AddEventAsync(Event events);
        Task<string?> UpdateEventAsync(Event events);
        Task<string> DeleteEventAsync(int eventId);
        Task<List<Event>> GetEventBySocietyId(int SocietyId);
    }
}
