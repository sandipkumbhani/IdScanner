using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Application.Services
{
    public class BlockService : IBlockService
    {
        private readonly IBlockRepository _blockRepository;   
        public BlockService(IBlockRepository blockRepository)
        {
            _blockRepository = blockRepository;
        }

        public async Task<List<Block>> GetAllBlockAsync()
        {
            return await _blockRepository.GetAllBlockAsync();
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
        public async Task<List<Block>> GetBlockBySocietyId(int? societyId)
        {
            return await _blockRepository.GetBlockBySocietyId(societyId);
        }

    }
}
