using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;


namespace SocPass.UI.Infrastructure.Provider
{
    public class BlockAdapter : IBlockAdapter
    {
        private readonly ICommonAdapter _commonAdapter;
        private readonly HttpClient _httpClinet;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;
        public BlockAdapter(ICommonAdapter commonAdapter, HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClinet = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
            _commonAdapter = commonAdapter;
        }
        public async Task<IList<Block>> GetBlockAsync()
        {
            return await _commonAdapter.GetAsync<IList<Block>>("Block/GetBlock");
        }
        public async Task<Block> GetBlockByIdAsync(int? blockid)
        {
            return await _commonAdapter.GetAsync<Block>($"Block/GetBlockById?blockid={blockid}");
        }
        public async Task<string> AddBlockAsync(Block block)
        {
            try
            {
                var response = await _commonAdapter.PostAsync<CommanResponseDto<Block>>("Block/Create-Block", block);

                if (response == null)
                    return "Unexpected null response from API.";

                if (response.IsSuccess)
                    return response.Message ?? "Block added successfully.";
                return response.Message ?? "Failed to create block.";
            }
            catch (HttpRequestException ex)
            {
                var statusCode = ex.StatusCode?.ToString() ?? "Unknown";
                var errorContent = ex.Message;
                if (statusCode == "Conflict")
                {
                    return $"Block '{block.BlockNumber}' already exists in this society.";
                }

                return $"Failed to create block. Server responded with {statusCode}: {errorContent}";
            }
            catch (Exception ex)
            {
                return $"Unexpected error occurred while creating block: {ex.Message}";
            }
        }
        public async Task<string> UpdateBlockAsync(Block block)
        {
            _httpClinet.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _globalClass.Token);

            var baseUrl = $"{apiCredential.url}Block/Update-Block";
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(block),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClinet.PutAsync(baseUrl, jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return responseContent;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return $"Block '{block.BlockNumber}' already exists in this society.";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return $"Not Found: {responseContent}";
            }
            return $"Error: {response.StatusCode} - {responseContent}";
        }

        public async Task<string> DeleteBlockAsync(int blockid)
        {
            return await _commonAdapter.DeleteAsync<string>($"Block/Delete-Block?blockid={blockid}");
        }
        public async Task<IList<Block>> GetBlockBySocietyId(int? societyId)
        {
            return await _commonAdapter.GetAsync<IList<Block>>($"Block/GetBlocksBySocietyId?societyId={societyId}");

        }
    }
}
