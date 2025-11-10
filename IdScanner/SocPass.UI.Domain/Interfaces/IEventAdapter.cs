using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Interfaces
{
    public interface IEventAdapter 
    {
        Task<IList<Event>> GetAllEventAsync();
        Task<string> AddEventAsync(Event events);
        Task<Event> GetEventByIdAsync(int? eventId);
        Task<string> UpdateEventAsync(Event events);
        Task<string> DeleteMenuAsync(int eventId);
        Task<IList<Event>> GetEventBySocietyId(int SocietyId);
        Task<IList<Event>> GetEventByUserId(int userid);

    }
}
