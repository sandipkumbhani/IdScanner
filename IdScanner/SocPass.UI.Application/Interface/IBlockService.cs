using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IBlockService
    {
        Task<List<Block>> GetAllBlockAsync();
        Task<Block?> GetBlockByIdAsync(int blockid);
        Task<string> DeleteBlockAsync(int blockid);
        Task<string> UpdateBlockAsync(Block block);
        Task<string> AddBlockAsync(Block block);
        Task<List<Block>> GetBlockBySocietyId(int? societyId);
    }
}
