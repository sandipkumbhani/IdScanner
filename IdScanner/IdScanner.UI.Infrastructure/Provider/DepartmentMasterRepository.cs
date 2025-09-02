using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;
using IdScanner.UI.Domain.Comman;
using IdScanner.UI.Domain.Helper;
using IdScanner.UI.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace IdScanner.UI.Infrastructure.Provider
{
    public class DepartmentMasterRepository : IDepartmentMasterRepository
    {
        private readonly HttpClient _httpClinet;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;

        public DepartmentMasterRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClinet = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }

        public async Task<List<Department>> GetAllDepartmentAsync()
        {
            var baseUrl = apiCredential.url + "Department/get-all-departments";
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Department>>(jsonString)!;
        }
        public async Task<Department> GetDepartmentByIdAsync(int? id)
        {
            var baseUrl = apiCredential.url + $"Department/GetById?departmentId={id}";
            var response = await _httpClinet.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Department>(jsonString)!;
        }
        public async Task<string> AddDepartmentAsync(Department departmentMaster)
        {
            var baseUrl = apiCredential.url + "Department/create";
            var departmentJson = JsonConvert.SerializeObject(departmentMaster);
            var requestContent = new StringContent(departmentJson, Encoding.UTF8, "application/json");
            var response = await _httpClinet.PostAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                var message = errorResponse?.ErrorMessage
                    ?? errorResponse?.Message
                    ?? "Failed to create department.";
                throw new Exception($"API Error ({response.StatusCode}): {message}");
            }
            return "Department Added Successfully.";
        }
        public async Task<string> UpdateDepartmentAsync(Department departmentMaster)
        {
            var baseUrl = apiCredential.url + $"Department/Update-User/{departmentMaster.DepartmentId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(departmentMaster), Encoding.UTF8, "application/json");
            var response = await _httpClinet.PutAsync(baseUrl, jsonContent);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> DeleteDepartmentAsync(int id)
        {
            var baseUrl = apiCredential.url + $"Department/Delete-department?id={id}";
            var response = await _httpClinet.DeleteAsync(baseUrl);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<List<Department>> GetDepartmentByCompanyId(int? companyId)
        {
            var baseUrl = apiCredential.url + $"Department/GetDepartmentByCompanyId?companyId={companyId}";
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Department>>(jsonString)!;
        }
    }
}
