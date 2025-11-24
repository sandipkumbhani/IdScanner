using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IBlockService
    {
        Task<IList<Block>> GetBlockAsync();
        Task<Block?> GetBlockByIdAsync(int blockid);
        Task<string> DeleteBlockAsync(int blockid);
        Task<string> UpdateBlockAsync(Block block);
        Task<string> AddBlockAsync(Block block);
        Task<IList<Block>> GetBlockBySocietyId(int? societyId);
    }
}
