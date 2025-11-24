using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private  readonly ISubscriptionAdapter _subscriptionRepository;
        public SubscriptionService(ISubscriptionAdapter subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }
        public async Task<IList<Subscription>> GetAllSubscription()
        {
            return await _subscriptionRepository.GetAllSubscriptionAsync();
        }
        public async Task<string> AddSubscriptionAsync(Subscription subscription)
        {
            return await _subscriptionRepository.AddSubscriptionAsync(subscription);
        }
        public async Task<Subscription> GetSubscriptionByIdAsync(int? subscriptionId)
        {
            return await _subscriptionRepository.GetSubsubscriptionByIdAsync(subscriptionId);
        }
        public async Task<Subscription> GetSubscriptionBySocietyIdAsync(int? societyId)
        {
            return await _subscriptionRepository.GetSubscriptionBySocietyIdAsync(societyId);
        }
        public async Task<string> UpdateSubscriptionAsync(Subscription subscription)
        {
            return await _subscriptionRepository.UpdateSubsubscriptionAsync(subscription);
        }
        public async Task<string> DeleteSubscriptionAsync(int subscriptionId)
        {
            return await _subscriptionRepository.DeleteSubsubscriptionAsync(subscriptionId);
        }
        public async Task<bool> ExistsSocietyDataAsync(int societyId, int subscriptionId)
        {
            return await _subscriptionRepository.ExistsSocietyDataAsync(societyId, subscriptionId);
        }
    }
}
