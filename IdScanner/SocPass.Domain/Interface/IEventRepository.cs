using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Interface
{
    public interface IEventRepository
    {
        Task<Event> AddEventAsync(Event events);
        Task<List<Event>> GetAllEventsAsync();
        Task<Event> UpdateEventAsync(Event events);
        Task DeleteEventAsync(Event events);
        Task<Event?> GetEventByIdAsync(int eventId);
        Task<List<Event>> GetEventListBySocietyAsync(int societyId);
        Task<List<Event>> GetEventListByUserIdAsync(int userid);
    }
}
