using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    public class FlatRepository :IFlatRepository
    {
        private readonly HttpClient _httpClinet;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;

        public FlatRepository(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClinet = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
        }

        public async Task<List<Flat>> GetAllFlatAsync()
        {
            var baseUrl = apiCredential.url + "Flat/getAllFlat";
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Flat>>(jsonString)!;
        }

        public async Task<List<Flat>> GetFlatByIdAsync(int? societyId, int blockId)
        {
            var baseUrl = apiCredential.url + $"Flat/GetFlatById?societyId={societyId}&blockId={blockId}";
            var response = await _httpClinet.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Flat>>(jsonString)!;
        }

        public async Task<List<Flat>> AddFlatAsync(Flat flat)
        {
            var baseUrl = apiCredential.url + "Flat/create";
            var flatJson = JsonConvert.SerializeObject(flat);
            var requestContent = new StringContent(flatJson, Encoding.UTF8, "application/json");
            var response = await _httpClinet.PostAsync(baseUrl, requestContent);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Flat>>(responseData)!;
        }

        //public async Task<List<Flat>> UpdateFlatAsync(Flat flat)
        //{
        //    var baseUrl = apiCredential.url + "Flat/Update-flat";
        //    var flatJson = JsonConvert.SerializeObject(flat);
        //    var requestContent = new StringContent(flatJson, Encoding.UTF8, "application/json");
        //    var response = await _httpClinet.PutAsync(baseUrl, requestContent);
        //    response.EnsureSuccessStatusCode();
        //    var responseData = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<List<Flat>>(responseData)!;
        //}

        public async Task<List<Flat>> UpdateFlatAsync(Flat flat)
        {
            var baseUrl = apiCredential.url + "Flat/Update-flat";
            var flatJson = JsonConvert.SerializeObject(flat);
            var requestContent = new StringContent(flatJson, Encoding.UTF8, "application/json");

            var response = await _httpClinet.PutAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            // Handle 200 OK
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseData);
                return result?.data?.ToObject<List<Flat>>() ?? new List<Flat>();
            }

            // Handle 409 Conflict (e.g., flat already exists)
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                try
                {
                    var error = JsonConvert.DeserializeObject<dynamic>(responseData);
                    string message = error?.message ?? "Flat already exists.";
                    throw new InvalidOperationException(message);
                }
                catch
                {
                    throw new InvalidOperationException("Flat already exists.");
                }
            }

            // Handle 400 BadRequest
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = JsonConvert.DeserializeObject<dynamic>(responseData);
                string message = error?.message ?? "Invalid flat data.";
                throw new InvalidOperationException(message);
            }

            // Handle 500 and other errors
            throw new Exception($"API Error ({response.StatusCode}): {responseData}");
        }


        public async Task<List<Flat>> GetFlatByBlockId(int blockid)
        {
            var baseUrl = $"{apiCredential.url}Flat/GetFlatByBlockid?blockid={blockid}";
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Flat>>(json)!;
        }
        public async Task<List<Flat>> GetQR(int blockid)
        {
            var baseUrl = $"{apiCredential.url}Flat/GetQR?blockid={blockid}";
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Flat>>(json)!;
        }
        public async Task<List<Flat>> GetGuestQR(int blockid)
        {
            var baseUrl = $"{apiCredential.url}Flat/GetGuestQR?blockid={blockid}";
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Flat>>(json)!;
        }
    }
}
