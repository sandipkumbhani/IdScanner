using SocPass.Domain.Model;

namespace SocPass.Domain.Interface
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> addsubscriptionAsync(Subscription subscription);
        Task<Subscription> GetById(int subscriptionId);
        Task UpdateSubscription(Subscription subscription);
        Task<List<Subscription>> GetAllSubscriptionAsync();
        Task DeleteSubscriptionAsync(int subscriptionId);
        Task<Subscription> GetSubscriptionBySocietyIdAsync(int societyId);
        Task<bool> ExistsSocietyDataAsync(int societyId, int subscriptionId);
    }
}
