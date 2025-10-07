using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Application.Interface
{
    public interface ISubscriptionService
    {
        Task<List<Subscription>> GetAllSubscription();
        Task<string> AddSubscriptionAsync(Subscription subscription);
        Task<Subscription> GetSubscriptionByIdAsync(int? subscriptionId);
        Task<string> UpdateSubscriptionAsync(Subscription subscription);
        Task<string> DeleteSubscriptionAsync(int subscriptionId);
        Task<Subscription> GetSubscriptionBySocietyIdAsync(int? societyId);
        Task<bool> ExistsSocietyDataAsync(int societyId, int subscriptionId);
    }
}
