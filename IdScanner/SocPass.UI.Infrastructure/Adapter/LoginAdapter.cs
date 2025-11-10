using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.DTO;
using SocPass.UI.Domain.Comman;
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
    public class LoginAdapter : ILoginAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;

        public LoginAdapter(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(_configuration);
        }

        public async Task<ResponseToken> CreateUserLoginAsync(LoginViewModel userModel)
        {
            try
            {
                string Response = string.Empty;
                var baseUrl = apiCredential.url + "Login/login";
                var user = JsonConvert.SerializeObject(userModel);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(baseUrl, requestContent);
                var responseData = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);

                if (response.IsSuccessStatusCode)
                {
                    if (responseModel != null && responseModel.Data != null)
                    {
                        var jsonData = JsonConvert.SerializeObject(responseModel.Data);
                        var responseToken = JsonConvert.DeserializeObject<ResponseToken>(jsonData);
                        if (responseToken != null && !string.IsNullOrEmpty(responseToken.Token))
                        {
                            return responseToken;
                        }

                    }
                }

                else
                {
                    if (responseModel != null && !string.IsNullOrEmpty(responseModel.ErrorMessage))
                    {
                        throw new Exception(responseModel.ErrorMessage);
                    }

                }
                throw new Exception(responseModel.ErrorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
