using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;
using System.Text;

namespace SocPass.UI.Infrastructure.Provider
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
        public async Task<List<User>> GetAllUsersAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "User/get-all-user";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<User>>(json)!;
        }
        public async Task<User> AddUserAsync(User user, int? flatId = null)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);

            var baseUrl = $"{apiCredential.url}User/create?flatId={flatId}";
            var userJson = JsonConvert.SerializeObject(user);
            var requestContent = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                try
                {
                    var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                    var message = errorResponse?.Message?.Trim() ?? "Duplicate entry.";

                    if (message.Contains("email", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("This email is already registered.");

                    if (message.Contains("flat", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("This flat is already assigned.");

                    throw new InvalidOperationException(message);
                }
                catch (JsonException)
                {
                    throw new InvalidOperationException("Duplicate entry found (email or flat).");
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                    var message = errorResponse?.ErrorMessage
                                ?? errorResponse?.Message
                                ?? responseData;

                    if (message.Contains("email", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("This email is already registered.");

                    if (message.Contains("flat", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("This flat is already assigned.");

                    throw new Exception($"API Error ({response.StatusCode}): {message}");
                }
                catch (JsonException)
                {
                    if (responseData.Contains("email", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("This email is already registered.");

                    if (responseData.Contains("flat", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("This flat is already assigned.");

                    throw new Exception($"API Error ({response.StatusCode}): {responseData}");
                }
            }

            try
            {
                var createdUser = JsonConvert.DeserializeObject<User>(responseData);
                return createdUser!;
            }
            catch (JsonException)
            {
                throw new Exception("Unexpected response format from API: " + responseData);
            }
        }

        public async Task<User> GetUsersByIdAsync(int? id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var baseUrl = apiCredential.url + $"User/GetById?userid={id}";
            var response = await _httpClient.GetAsync(baseUrl);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to get user. Status code: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(jsonString)!;
        }
        public async Task<User> UpdateUserAsync(User user, int? flatId = null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"User/Update-User/{user.UserId}?flatId={flatId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(baseUrl, jsonContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to update user. Status code: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(jsonString)!;
        }
        public async Task<string> DeleteUserAsync(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = $"{apiCredential.url}User/Delete-User?id={id}";
            var response = await _httpClient.DeleteAsync(baseUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to delete user. Status code: {response.StatusCode}");
            }
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<List<UserRole>> GetAllUserRoleAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "UserRole/get-all-userRole";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<UserRole>>(json)!;
        }
        public async Task<UserRole> GetRoleNameByIdAsync(long? id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var baseUrl = apiCredential.url + $"UserRole/GetUserRoleById?id={id}";
            var response = await _httpClient.GetAsync(baseUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to get user. Status code: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserRole>(jsonString)!;
        }
    }
}

