using IdScanner.Domain.Model;
using IdScanner.UI.Domain.Helper;
using IdScanner.UI.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    }
}
