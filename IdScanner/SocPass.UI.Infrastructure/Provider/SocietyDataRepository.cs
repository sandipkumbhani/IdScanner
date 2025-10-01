using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;
using CommanResponseDto = SocPass.UI.Domain.Comman.CommanResponseDto;

namespace SocPass.UI.Infrastructure.Provider
{
    public class SocietyDataRepository : ISocietyDataRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;

        public SocietyDataRepository(HttpClient httpCleint, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClient = httpCleint;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
        }

        public async Task<List<SocietyData>> GetAllSocietyData()
        {
            var baseUrl = apiCredential.url + "SocietyData/Get-All-SocietyData";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<SocietyData>>(json)!;
        }

        public async Task<SocietyData> GetSocietyDataByIdAsync(int? societyDataId)
        {
            var baseUrl = apiCredential.url + $"SocietyData/GetBySocietyDataId?societyDataId={societyDataId}";
            var response = await _httpClient.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SocietyData>(jsonString)!;
        }
        public async Task<string> AddSocietyDataAsync(SocietyDataCreateRequest societyData)
        {
            var baseUrl = apiCredential.url + "SocietyData/Add-Societydata";
            var userJson = JsonConvert.SerializeObject(societyData);
            var requestContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                var message = errorResponse?.ErrorMessage
                    ?? errorResponse?.Message
                    ?? "Failed to create SocietyData.";

                throw new Exception($"API Error ({response.StatusCode}): {message}");
            }
            return "SocietyData Added Successfully.";
        }
        public async Task<string> UpdateSocietyDataAsync(SocietyData societyData)
        {
            var baseUrl = apiCredential.url + $"SocietyData/Update-Societydata/{societyData.SocietyDataId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(societyData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(baseUrl, jsonContent);
                return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> DeleteSocietyDataAsync(int societyDataId)
        {
            var baseUrl = apiCredential.url + $"SocietyData/Delete-SocietyData?societyDataId={societyDataId}";
            var response = await _httpClient.DeleteAsync(baseUrl);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<SocietyData>> GetSocietyDataByFlatId(int flatId)
        {
            var baseUrl = apiCredential.url + $"SocietyData/Get-SocietyData-By-FlatId?flatId={flatId}";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<SocietyData>>(json)!;
        }
    }
}
