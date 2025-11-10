using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Interface
{
    public interface ISubscriptionService
    {
        Task<Subscription> addsubscriptionAsync(Subscription subscription);
        Task<Subscription> GetById(int subscriptionId);
        Task<Subscription> UpdateSubscriptionAsync(Subscription subscription);
        Task DeleteSubscriptionAsync(int subscriptionId);
        Task<List<Subscription>> GetAllSubscription();
        Task<Subscription> GetSubscriptionBySocietyIdAsync(int societyId);

        Task<bool> ExistsSocietyDataAsync(int societyId, int subscriptionId);

    }
}
