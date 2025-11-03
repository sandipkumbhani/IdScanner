using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Infrastructure.Provider
{
    public class UserFlatMappingRepository : IUserFlatMappingRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;
        public UserFlatMappingRepository(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
        }
        public async Task<List<QRCodeMaster>> GetQrByUserId(int? userid,int EventId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = $"{apiCredential.url}UserFlatMapping/GetQrByUserId?userid={userid}&EventId={EventId}";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<QRCodeMaster>>(jsonString)!;
        }
    }
}
