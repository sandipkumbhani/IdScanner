using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Interfaces;


namespace SocPass.UI.Infrastructure.Provider
{
    public class BlockAdapter : IBlockAdapter
    {
        private readonly ICommonAdapter _commonAdapter;
        public BlockAdapter(ICommonAdapter commonAdapter)
        {
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
            return await _commonAdapter.PutAsync($"Block/Update-Block", block);
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
