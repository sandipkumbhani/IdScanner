using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Infrastructure.Provider
{
    public class ReportRepository : IReportRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;

        public ReportRepository(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
        }

        public async Task<object> GetReportAsync(int blockId, int eventId, DateTime startDate)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);

            var baseUrl = apiCredential.url + $"Report/GetReport?blockId={blockId}&eventId={eventId}&startDate={startDate:yyyy-MM-dd}";

            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(result);
        }
    }
}
