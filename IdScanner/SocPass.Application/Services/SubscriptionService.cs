using SocPass.Application.Interface;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ISocietyRepository _societyRepository;
        public SubscriptionService(ISubscriptionRepository subscriptionRepository, ISocietyRepository societyRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _societyRepository = societyRepository;
        }
        public async Task<Subscription> addsubscriptionAsync(Subscription subscription)
        {
            //var existingSubscription = await _subscriptionRepository.GetById(subscription.SocietyId);
            //if (existingSubscription != null)
            //{
            //    return null; 
            //}
            var newSubscription = new Subscription
            {
                SocietyId = subscription.SocietyId,
                StartFrom = subscription.StartFrom,
                EndTo = subscription.EndTo,
                //AllowNoOfName = subscription.AllowNoOfName,
                //AllowNoOfContact = subscription.AllowNoOfContact,
                //AllowNoOfEmail = subscription.AllowNoOfEmail,
                IsActive = true,
                InsertDate = DateTime.Now,
                InsertBy = 0,
                UpdateDate = DateTime.Now,
                UpdateBy = 0,
            };
           return await _subscriptionRepository.addsubscriptionAsync(newSubscription);
        }
        public async Task<Subscription> GetById(int subscriptionId)
        {
            return await _subscriptionRepository.GetById(subscriptionId);
        }

        public async Task<Subscription> GetSubscriptionBySocietyIdAsync(int societyId)
        {
            return await _subscriptionRepository.GetSubscriptionBySocietyIdAsync(societyId);
        }

        public async Task<Subscription> UpdateSubscriptionAsync(int subscriptionId, Subscription subscription)
        {
            var subscriptionUpdate = await _subscriptionRepository.GetById(subscriptionId);
            if (subscriptionUpdate == null)
            {
                throw new Exception($"Menu Master with ID {subscriptionId} not found.");
            }
            subscriptionUpdate.StartFrom= subscription.StartFrom;
            subscriptionUpdate.EndTo = subscription.EndTo;
            subscriptionUpdate.IsActive = true;
            subscriptionUpdate.InsertBy = 1;
            subscriptionUpdate.InsertDate = DateTime.UtcNow;
            subscriptionUpdate.UpdateBy = 1;
            subscriptionUpdate.UpdateDate = DateTime.UtcNow;
            subscriptionUpdate.AllowNoOfName = subscription.AllowNoOfName;
            subscriptionUpdate.AllowNoOfContact = subscription.AllowNoOfContact;
            subscriptionUpdate.AllowNoOfEmail = subscription.AllowNoOfEmail;
            await _subscriptionRepository.UpdateSubscription(subscriptionUpdate);
            return subscriptionUpdate;
        }
        public async Task<List<Subscription>> GetAllSubscription()
        {
            var getall = await _subscriptionRepository.GetAllSubscriptionAsync();
            return getall ?? new List<Subscription>();
        }
        public async Task DeleteSubscriptionAsync(int subscriptionId)
        {
            var delete = await _subscriptionRepository.GetById(subscriptionId);
            if (delete == null)
            {
                throw new KeyNotFoundException($"Menu Master ID {subscriptionId} not found.");
            }
            await _subscriptionRepository.DeleteSubscriptionAsync(subscriptionId);
        }
        public async Task<bool> ExistsSocietyDataAsync(int societyId, int subscriptionId)
        {
            return await _subscriptionRepository.ExistsSocietyDataAsync(societyId, subscriptionId);
        }
    }
}
