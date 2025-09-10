using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;
using IdScanner.UI.Domain.Comman;
using IdScanner.UI.Domain.Helper;
using IdScanner.UI.Domain.Interfaces;
using IdScanner.UI.Domain.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace IdScanner.UI.Infrastructure.Provider
{
    public class CompanyMasterRepository : ICompanyMasterRepository
    {
        private readonly HttpClient _httpClinet;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;

        public CompanyMasterRepository(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClinet = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
        }

        public async Task<List<CompanyMaster>> GetAllCompanyAsync()
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "CompanyMaster/get-all-CompanyMaster";
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CompanyMaster>>(jsonString)!;
        }
        public async Task<CompanyMaster> GetCompanyByIdAsync(int? id)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"CompanyMaster/GetCompanyMasterById?id={id}";
            var response = await _httpClinet.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CompanyMaster>(jsonString)!;
        }
        public async Task<string> AddCompanyAsync(CompanyMaster companyMaster)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "CompanyMaster/create";
            var companyJson = JsonConvert.SerializeObject(companyMaster);
            var requestContent = new StringContent(companyJson,Encoding.UTF8,"application/json");
            var response = await _httpClinet.PostAsync(baseUrl,requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if(!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                var message = errorResponse?.ErrorMessage
                    ?? errorResponse?.Message
                    ?? "Failed to create company.";
                throw new Exception($"API Error ({response.StatusCode}): {message}");
            }
            return "Company Added Successfully.";
        }

        public async Task<string> UpdateCompanyAsync(CompanyMaster companyMaster)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"CompanyMaster/Update-CompanyMaster/{companyMaster.CompanyId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(companyMaster), Encoding.UTF8, "application/json");
            var response = await _httpClinet.PutAsync(baseUrl, jsonContent);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteCompanyAsync(int id)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"CompanyMaster/Delete-CompanyMaster?companyid={id}";
            var response = await _httpClinet.DeleteAsync(baseUrl);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
