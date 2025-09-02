using Google.Apis.Drive.v3.Data;
using IdScanner.Domain.Model;
using IdScanner.UI.Domain.Comman;
using IdScanner.UI.Domain.Helper;
using IdScanner.UI.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;
using IdScanner.UI.Domain.Helper;
using IdScanner.UI.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace IdScanner.UI.Infrastructure.Provider
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;

        public UserDataRepository(HttpClient httpCleint, IConfiguration configuration)
        {
            _httpClient = httpCleint;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }
        public async Task<List<UserData>> GetAllUserDetailsAsync()
        {
            var baseUrl = apiCredential.url + "UserData/Get-All-User-Data";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<UserData>>(json)!;
        }

        public async Task<string> UpdateUserAsync(UserData userData)
        {
            var baseUrl = apiCredential.url + $"UserData/Update-UserData/{userData.UserDataId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(userData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(baseUrl, jsonContent);    
            return await response.Content.ReadAsStringAsync();

        }

        public async Task<string> DeleteUserAsync(int id)
        {
            var baseUrl = apiCredential.url + $"UserData/Delete-User-Data?id={id}";
            var response = await _httpClient.DeleteAsync(baseUrl);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<UserData> AddUserDataAsync(UserData userData)
        {
            var baseUrl = apiCredential.url + "UserData/create";

            var userJson = JsonConvert.SerializeObject(userData);
            var requestContent = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                var message = errorResponse?.ErrorMessage
                              ?? errorResponse?.Message
                              ?? "Failed to create menu.";

                throw new Exception($"API Error ({response.StatusCode}): {message}");
            }
            try
            {
                var createdMappings = JsonConvert.DeserializeObject<UserData>(responseData);
                return createdMappings;
            }
            catch (JsonException)
            {
                throw new Exception($"Unexpected response format. Raw response: {responseData}");
            }
        }
        public async Task<UserData> GetUserDataByIdAsync(int? userid)
        {
            var baseUrl = apiCredential.url + $"UserData/GetByUserDataById?userId={userid}";
            var response = await _httpClient.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserData>(jsonString)!;
        }

    }
}
