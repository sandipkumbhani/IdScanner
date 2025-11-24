using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Application.Services
{
    public class BlockService : IBlockService
    {
        private readonly IBlockAdapter _blockRepository;   
        public BlockService(IBlockAdapter blockRepository)
        {
            _blockRepository = blockRepository;
        }

        public async Task<IList<Block>> GetBlockAsync()
        {
             return await _blockRepository.GetBlockAsync();
        }
        public async Task<string> DeleteBlockAsync(int blockid)
        {
            return await _blockRepository.DeleteBlockAsync(blockid);
        }
        public async Task<Block?> GetBlockByIdAsync(int blockid)
        {
            return await _blockRepository.GetBlockByIdAsync(blockid);
        }
        public async Task<string> AddBlockAsync(Block block)
        {
            return await _blockRepository.AddBlockAsync(block);
        }
        public async Task<string> UpdateBlockAsync(Block block)
        {
            return await _blockRepository.UpdateBlockAsync(block);
        }
        public async Task<IList<Block>> GetBlockBySocietyId(int? societyId)
        {
            return await _blockRepository.GetBlockBySocietyId(societyId);
        }
    }
}
