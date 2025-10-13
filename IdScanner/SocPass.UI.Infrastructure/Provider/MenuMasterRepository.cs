using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;
using System.Net.Http;

namespace SocPass.UI.Infrastructure.Provider
{
    public class MenuMasterRepository : IMenuMasterRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private readonly GlobalClass _globalClass;
        public MenuMasterRepository(HttpClient httpCleint, IConfiguration configuration,GlobalClass globalClass)
        {
            _httpClient = httpCleint;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass; 
        }
        public async Task<List<MenuMaster>> GetAllMenuAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "MenuMaster/Get-All-Menu-Master";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<MenuMaster>>(json)!;
        }
        public async Task<MenuMaster> GetMenuByIdAsync(int? id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"MenuMaster/GetByMenuId?id={id}";
            var response = await _httpClient.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MenuMaster>(jsonString)!;
        }
        public async Task<string> AddMenuAsync(MenuMaster menuMaster)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "MenuMaster/Menu-Master";
            var userJson = JsonConvert.SerializeObject(menuMaster);
            var requestContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                var message = errorResponse?.ErrorMessage
                    ?? errorResponse?.Message
                    ?? "Failed to create user.";

                throw new Exception($"API Error ({response.StatusCode}): {message}");
            }
            return "Menu Added Successfully.";
        }
        public async Task<string> UpdateMenuAsync(MenuMaster menuMaster)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"MenuMaster/Update-Menu/{menuMaster.MenuId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(menuMaster), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(baseUrl, jsonContent);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> DeleteMenuAsync(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"MenuMaster/Delete-Menu-Master?id={id}";
            var response = await _httpClient.DeleteAsync(baseUrl);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<List<MenuMaster>> GetMenuByUserIdAsync(int userId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"MenuMaster/GetMenusByUserId?userId={userId}";
            var response = await _httpClient.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<MenuMaster>>(jsonString)!;
        }
    }
}

