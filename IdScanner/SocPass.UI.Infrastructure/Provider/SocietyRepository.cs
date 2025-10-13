using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Infrastructure.Provider
{
    public class SocietyRepository : ISocietyRepository
    {
        private readonly HttpClient _httpClinet;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;

        public SocietyRepository(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClinet = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
        }
        public async Task<List<Society>> GetAllSocietyAsync(int userId)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"Society/getAllSociety?userId={userId}";
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Society>>(jsonString)!;
        }

        public async Task<List<Society>> GetAllSocietyAsync()
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"Society/getAllSociety";
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Society>>(jsonString)!;
        }   

        public async Task<Society> GetSocietyByIdAsync(int? societyId)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"Society/GetSocietyById?societyId={societyId}";
            var response = await _httpClinet.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Society>(jsonString)!;
        }
        public async Task<string> AddSocietyAsync(Society society)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "Society/create";
            var societyJson = JsonConvert.SerializeObject(society);
            var requestContent = new StringContent(societyJson, Encoding.UTF8, "application/json");
            var response = await _httpClinet.PostAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                var message = errorResponse?.ErrorMessage
                    ?? errorResponse?.Message
                    ?? "Failed to create Society.";
                throw new Exception($"API Error ({response.StatusCode}): {message}");
            }
            return "Society Added Successfully.!";
        }

        public async Task<string> UpdateSocietyAsync(Society society)
        {
            if (society.SocietyId <= 0)
            {
                throw new Exception("Invalid SocietyId. Cannot update society without valid ID.");
            }

            _httpClinet.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);

            var baseUrl = $"{apiCredential.url}Society/Update-society/{society.SocietyId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(society), Encoding.UTF8, "application/json");

            var response = await _httpClinet.PutAsync(baseUrl, jsonContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API Error ({response.StatusCode}): {responseData}");
            }

            return responseData;
        }

        public async Task<string> DeleteSocietyAsync(int societyId)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"Society/Delete-Society?societyId={societyId}";
            var response = await _httpClinet.DeleteAsync(baseUrl);
            return await response.Content.ReadAsStringAsync();
        }

    }
}
