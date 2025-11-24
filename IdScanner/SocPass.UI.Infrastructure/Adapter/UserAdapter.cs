using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;
using System.Text;

namespace SocPass.UI.Infrastructure.Provider
{
    public class UserAdapter : IUserAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private readonly GlobalClass _globalClass;
        private readonly ICommonAdapter _commonAdapter;
        public UserAdapter(HttpClient httpCleint, IConfiguration configuration, GlobalClass globalClass, ICommonAdapter commonAdapter)
        {
            _httpClient = httpCleint;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
            _commonAdapter = commonAdapter;
        }
        public async Task<IList<User>> GetUsersAsync()
        {
            return await _commonAdapter.GetAsync<IList<User>>($"User/get-user");
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
            return await _commonAdapter.GetAsync<User>($"User/GetById?userid={id}");
        }
        public async Task<string> UpdateUserAsync(User user, int? flatId = null)
        {
            return await _commonAdapter.PutAsync($"User/Update-User?flatId={flatId}", user);
        }
        public async Task<string> DeleteUserAsync(int id)
        {
            return await _commonAdapter.DeleteAsync<string>($"User/Delete-User?id={id}");
        }
        public async Task<IList<UserRole>> GetAllUserRoleAsync()
        {
            return await _commonAdapter.GetAsync<IList<UserRole>>($"UserRole/get-all-userRole");
        }
        public async Task<UserRole> GetRoleNameByIdAsync(long? id)
        {
            return await _commonAdapter.GetAsync<UserRole>($"UserRole/GetUserRoleById?id={id}");
        }
    }
}

