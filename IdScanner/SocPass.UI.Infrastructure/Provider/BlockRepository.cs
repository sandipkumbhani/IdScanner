using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;


namespace SocPass.UI.Infrastructure.Provider
{
    public class BlockRepository : IBlockRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        public BlockRepository(HttpClient httpCleint, IConfiguration configuration)
        {
            _httpClient = httpCleint;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }

        public async Task<List<Block>> GetAllBlockAsync()
        {
            var baseUrl = apiCredential.url + "Block/GetAllBlock";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Block>>(json)!;
        }
        public async Task<Block> GetBlockByIdAsync(int? blockid)
        {
            var baseUrl = apiCredential.url + $"Block/GetBlockById?blockid={blockid}";
            var response = await _httpClient.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Block>(jsonString)!;
        }
        public async Task<string> AddBlockAsync(Block block)
        {
            var baseUrl = apiCredential.url + "Block/Create-Block";
            var userJson = JsonConvert.SerializeObject(block);
            var requestContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                var message = errorResponse?.ErrorMessage
                    ?? errorResponse?.Message
                    ?? "Failed to create user.";

                throw new Exception($"API Error ({response.StatusCode}): {message}");
            }
            return "Menu Added Successfully.";
        }
        public async Task<string> UpdateBlockAsync(Block block)
        {
            var baseUrl = apiCredential.url + $"Block/Update-Block/{block.BlockId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(block), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(baseUrl, jsonContent);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> DeleteBlockAsync(int blockid)
        {
            var baseUrl = apiCredential.url + $"Block/Delete-Block?blockid={blockid}";
            var response = await _httpClient.DeleteAsync(baseUrl);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<List<Block>> GetBlockBySocietyId(int? societyId)
        {
            var baseUrl = apiCredential.url + $"Block/GetBlocksBySocietyId?societyId={societyId}";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Block>>(jsonString)!;
        }   
    }
}
