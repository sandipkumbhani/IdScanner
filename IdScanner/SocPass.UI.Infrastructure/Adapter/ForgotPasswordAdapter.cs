using Microsoft.Extensions.Configuration;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Infrastructure.Provider
{
    public class ForgotPasswordAdapter : IForgotPasswordAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;

        public ForgotPasswordAdapter(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }

        public async Task<string> ForgotPasswordByEmailAsync(string email)
        {
            var baseUrl = $"{apiCredential.url}ForgotPassword/forgot-password?email={email}";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();

        }
    }
}
