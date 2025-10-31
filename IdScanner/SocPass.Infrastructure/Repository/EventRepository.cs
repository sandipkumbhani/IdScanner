using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Infrastructure.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context; 
        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Event> AddEventAsync(Event events)
        {
            _context.Events.Add(events);
            await _context.SaveChangesAsync();
            return events;
        }
        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.Society)    
                .Where(e => e.IsActive == true) 
                .OrderByDescending(e => e.StartDate).ToListAsync();
        }
        public async Task<Block> GetBlockByIdAsync(int blockid)
        {
            return await _context.blocks.Include(e => e.Society)
                  .Where(x => x.IsActive == true).FirstOrDefaultAsync(e => e.BlockId == blockid);
        }
        public async Task<Event?> GetEventByIdAsync(int eventId)
        {
            return await _context.Events
                .Include(e => e.Society)
                .Where(x => x.IsActive == true)
                .FirstOrDefaultAsync(e => e.EventId == eventId);
        }
        public async Task<List<Event>> GetEventListBySocietyAsync(int societyId)
        {
            return await _context.Events
                .Include(e => e.Society)
                .Where(e => e.SocietyId == societyId && e.IsActive == true)
                .ToListAsync();
        }

        public async Task<Event> UpdateEventAsync(Event events)
        {
            _context.Events.Update(events);
            await _context.SaveChangesAsync();
            return events;
        }
        public async Task DeleteEventAsync(Event events)
        {
            var existingEvent = await _context.Events.FindAsync(events.EventId);
            if (existingEvent != null)
            {
                existingEvent.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
