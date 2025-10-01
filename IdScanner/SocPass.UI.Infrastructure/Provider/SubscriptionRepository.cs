using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Infrastructure.Provider
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;

        public SubscriptionRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }
        public async Task<List<Subscription>> GetAllSubscriptionAsync()
        {
            var baseUrl = apiCredential.url + "Subscription/Get-All-Subscription";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Subscription>>(json)!;
        }
        public async Task<string> AddSubscriptionAsync(Subscription subscription)
        {
            var baseUrl = apiCredential.url + "Subscription/create";
            var societyJson = JsonConvert.SerializeObject(subscription);
            var requestContent = new StringContent(societyJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                var message = errorResponse?.ErrorMessage
                    ?? errorResponse?.Message
                    ?? "Failed to create Society.";
                throw new Exception($"API Error ({response.StatusCode}): {message}");
            }
            return "SubScription Added Successfully.!";
        }
        public async Task<Subscription> GetSubsubscriptionByIdAsync(int? subscriptionId)
        {
            var baseUrl = apiCredential.url + $"Subscription/GetById?subscriptionId={subscriptionId}";
            var response = await _httpClient.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Subscription>(jsonString)!;
        }
        public async Task<string> UpdateSubsubscriptionAsync(Subscription subscription)
        {
            //_httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"Subscription/Update-Subscription/{subscription.SubscriptionId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(subscription), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(baseUrl, jsonContent);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteSubsubscriptionAsync(int subscriptionId)
        {
            //_httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"Subscription/Delete-Subscription?subscriptionId={subscriptionId}";
            var response = await _httpClient.DeleteAsync(baseUrl);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
