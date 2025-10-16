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
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "SocietyData/Get-All-SocietyData";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<SocietyData>>(json)!;
        }

        public async Task<SocietyData> GetSocietyDataByIdAsync(int? societyDataId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"SocietyData/GetBySocietyDataId?societyDataId={societyDataId}";
            var response = await _httpClient.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SocietyData>(jsonString)!;
        }
        public async Task<string> AddSocietyDataAsync(SocietyDataCreateRequest societyData)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "SocietyData/Add-Societydata";
            var userJson = JsonConvert.SerializeObject(societyData);
            var requestContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            try
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseData);
                bool isSuccess = result.success;
                string message = result.message;

                // Return message string only
                return message;
            }
            catch
            {
                // fallback if JSON cannot be parsed
                if (!response.IsSuccessStatusCode)
                    return $"Error: Failed to create Society Data (HTTP {response.StatusCode}).";

                return "Society Data added successfully.";
            }
        }
        public async Task<string> UpdateSocietyDataAsync(SocietyData societyData)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"SocietyData/Update-Societydata/{societyData.SocietyDataId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(societyData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(baseUrl, jsonContent);
                return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> DeleteSocietyDataAsync(int societyDataId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"SocietyData/Delete-SocietyData?societyDataId={societyDataId}";
            var response = await _httpClient.DeleteAsync(baseUrl);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<SocietyData>> GetSocietyDataByFlatId(int flatId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"SocietyData/Get-SocietyData-By-FlatId?flatId={flatId}";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<SocietyData>>(json)!;
        }
    }
}
