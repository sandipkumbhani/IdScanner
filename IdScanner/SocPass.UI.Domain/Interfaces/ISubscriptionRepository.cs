using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<List<Subscription>> GetAllSubscriptionAsync();
        Task<string> AddSubscriptionAsync(Subscription subscription);
        Task<string> DeleteSubsubscriptionAsync(int subscriptionId);
        Task<string> UpdateSubsubscriptionAsync(Subscription subscription);
        Task<Subscription> GetSubsubscriptionByIdAsync(int? subscriptionId);
    }
}
