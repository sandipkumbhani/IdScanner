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

namespace SocPass.UI.Infrastructure
{
    public class ResetPasswordRepossitory : IResetPasswordRepossitory
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;

        public ResetPasswordRepossitory(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }
        public async Task<string> UpdatePasswordAsync(ResetPasswordModel resetPasswordModel)
        {
            var baseUrl = $"{apiCredential.url}ForgotPassword/reset-password";
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(resetPasswordModel),
            Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync(baseUrl, jsonContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
