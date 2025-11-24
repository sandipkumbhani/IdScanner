using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Infrastructure.Provider
{
    public class SubscriptionAdapter : ISubscriptionAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private readonly GlobalClass _globalClass;
        private readonly ICommonAdapter _commonAdapter;

        public SubscriptionAdapter(HttpClient httpClient, IConfiguration configuration,GlobalClass globalClass, ICommonAdapter commonAdapter)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
            _commonAdapter = commonAdapter;
        }
        public async Task<IList<Subscription>> GetAllSubscriptionAsync()
        {
            return await _commonAdapter.GetAsync<IList<Subscription>>($"Subscription/Get-All-Subscription");
        }
        public async Task<string> AddSubscriptionAsync(Subscription subscription)
        {
            try
            {
                var response = await _commonAdapter.PostAsync<CommanResponseDto<Subscription>>($"Subscription/create", subscription);

                return response?.Message ?? "Subscription added successfully.";
            }
            catch (HttpRequestException ex)
            {
                return $"Failed to connect to the server: {ex.Message}";
            }
            catch (JsonException ex)
            {
                return $"Invalid response format from server: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred: {ex.Message}";
            }
        }
        public async Task<Subscription> GetSubsubscriptionByIdAsync(int? subscriptionId)
        {
            return await _commonAdapter.GetAsync<Subscription>($"Subscription/GetById?subscriptionId={subscriptionId}");
        }
        public async Task<Subscription> GetSubscriptionBySocietyIdAsync(int? societyId)
        {
            return await _commonAdapter.GetAsync<Subscription >($"Subscription/GetBySocietyId?societyId={societyId}");
        }
        public async Task<string> UpdateSubsubscriptionAsync(Subscription subscription)
        {
            return await _commonAdapter.PutAsync($"Subscription/Update-Subscription", subscription);
        }
        public async Task<string> DeleteSubsubscriptionAsync(int subscriptionId)
        {
            return await _commonAdapter.DeleteAsync<string>($"Subscription/Delete-Subscription?subscriptionId={subscriptionId}");
        }

        public async Task<bool> ExistsSocietyDataAsync(int societyId, int subscriptionId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"Subscription/GetExistsSocietyData?societyId={societyId}&subscriptionId={subscriptionId}";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }
    }
}
