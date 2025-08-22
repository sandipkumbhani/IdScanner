using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.UI.Domain.Helper;
using IdScanner.UI.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IdScanner.UI.Infrastructure.Provider
{
    public class ForgotPasswordRepository : IForgotPasswordRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;

        public ForgotPasswordRepository(HttpClient httpClient, IConfiguration configuration)
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
