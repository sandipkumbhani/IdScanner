using Emertec.UI.Domain.Comman;
using Emertec.UI.Domain.Helper;
using Emertec.UI.Domain.Interfaces;
using Emertec.UI.Domain.Model;
using MicroService_Template.Domain.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Emertec.UI.Infrastructure.Provider
{
    public class UserRepository : IUserRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;
        public UserRepository(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
        }
        public async Task<List<ModelUsers>> GetAllUsersAsync()
        {
            var baseUrl = apiCredential.url + "User/get-all-user";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ModelUsers>>(json)!;
        }
        public async Task<ModelUsers> AddUserAsync(ModelUsers user)
        {
            var baseUrl = apiCredential.url + "User/create";

            var userJson = JsonConvert.SerializeObject(user);
            var requestContent = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Try parsing as JSON error response
                try
                {
                    var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);

                    var message = errorResponse?.ErrorMessage
                                  ?? errorResponse?.Message
                                  ?? responseData; // fallback to raw response text

                    if (message.Contains("email", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("This email is already registered.");
                    }

                    throw new Exception($"API Error ({response.StatusCode}): {message}");
                }
                catch (JsonException)
                {
                    // If not JSON, treat it as plain text
                    if (responseData.Contains("email", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("This email is already registered.");
                    }
                    throw new Exception($"API Error ({response.StatusCode}): {responseData}");
                }
            }

            // ✅ Only try to deserialize if success
            try
            {
                var createdUser = JsonConvert.DeserializeObject<ModelUsers>(responseData);
                return createdUser!;
            }
            catch (JsonException)
            {
                throw new Exception("Unexpected response format from API: " + responseData);
            }
        }

        public async Task<ModelUsers> GetUsersByIdAsync(long? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var baseUrl = apiCredential.url + $"User/GetById?userid={id}";
            var response = await _httpClient.GetAsync(baseUrl);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to get user. Status code: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ModelUsers>(jsonString)!;
        }
        public async Task<ModelUsers> UpdateUserAsync(ModelUsers user)
        {
            var baseUrl = apiCredential.url + $"User/Update-User/{user.UserId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(baseUrl, jsonContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to update user. Status code: {response.StatusCode}");

            var jsonString =  await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ModelUsers>(jsonString)!;
        }
        public async Task<string> DeleteUserAsync(int id)
        {
            var baseUrl = $"{apiCredential.url}User/Delete-User?id={id}";
            var response = await _httpClient.DeleteAsync(baseUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to delete user. Status code: {response.StatusCode}");
            }
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<List<ModelUserRole>> GetAllUserRoleAsync()
        {
            var baseUrl = apiCredential.url + "UserRole/get-all-userRole";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ModelUserRole>>(json)!;
        }
        public async Task<ModelUserRole> GetRoleNameByIdAsync(long? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var baseUrl = apiCredential.url + $"UserRole/GetUserRoleById?id={id}";
            var response = await _httpClient.GetAsync(baseUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to get user. Status code: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ModelUserRole>(jsonString)!;
        }
    }
}

