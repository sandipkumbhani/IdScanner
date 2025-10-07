using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;

namespace SocPass.Infrastructure.Repository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _context;
        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Subscription> addsubscriptionAsync(Subscription subscription)
        {
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }
        public async Task<Subscription> GetById(int subscriptionId)
        {
          return  await _context.Subscriptions.FirstOrDefaultAsync(e=>e.SubscriptionId == subscriptionId);
        }

        public async Task<Subscription> GetSubscriptionBySocietyIdAsync(int societyId)
        {
            return await _context.Subscriptions.FirstOrDefaultAsync(e => e.SocietyId == societyId);
        }
        public async Task UpdateSubscription(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Subscription>> GetAllSubscriptionAsync()
        {
            return await _context.Subscriptions
                                 .Include(u => u.Society)
                                 .Where(u => u.IsActive == true)
                                 .ToListAsync();
        }
        public async Task DeleteSubscriptionAsync(int subscriptionId)
        {
            var Deletesub = await _context.Subscriptions.FindAsync(subscriptionId);
            if (Deletesub != null)
            {
                Deletesub.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsSocietyDataAsync(int societyId, int subscriptionId)
        {
            return await _context.Subscriptions.AnyAsync(e => e.SocietyId == societyId && e.SubscriptionId != subscriptionId);
        }
    }
}
