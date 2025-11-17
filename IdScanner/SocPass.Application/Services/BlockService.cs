using Microsoft.AspNetCore.Http;
using SocPass.Application.Interface;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System.Security.Claims;

namespace SocPass.Application.Services
{
    public class BlockService : IBlockService
    {
        private readonly IBlockRepository _blockRepository;
        private readonly ISocietyRepository _societyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BlockService(IBlockRepository blockRepository, IHttpContextAccessor httpContextAccessor, ISocietyRepository societyRepository)
        {
            _blockRepository = blockRepository;
            _httpContextAccessor = httpContextAccessor;
            _societyRepository = societyRepository;
        }
        public async Task<Block> CreateBlockAsync(Block block)
        {
           
            var existingBlocks = await _blockRepository.GetBlocksBySocietyIdAsync(block.SocietyId);
            var checkBlockExisting = existingBlocks
                .FirstOrDefault(b => b.BlockNumber.Trim().ToLower() == block.BlockNumber.Trim().ToLower());

            if (checkBlockExisting != null)
            {
                throw new InvalidOperationException($"Block '{block.BlockNumber}' already exists in this society.");
            }

            var newBlock = new Block
            {
                BlockNumber = block.BlockNumber,
                SocietyId = block.SocietyId,
                IsActive = true,
                InsertBy = 1,
                InsertDate = DateTime.Now,
                UpdateBy = 1,
                UpdateDate = DateTime.Now
            };

            return await _blockRepository.CreateBlockAsync(newBlock);
        }
        public async Task<List<Block>> GetAllBlockAsync()
        {
            var role = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var newBlock = new List<Block>();
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                 newBlock  = await _blockRepository.GetAllBlockAsync();
            }
            else
            {
                var societies = await _societyRepository.GetAllSocietyAsync(userId);
                var society = societies.FirstOrDefault();

                if (society != null)
                {
                    newBlock = await _blockRepository.GetBlocksBySocietyIdAsync(society.SocietyId);
                           
                }
                else
                {
                    newBlock = new List<Block>();
                }
            }
            return newBlock ?? new List<Block>();
        }

        public async Task<Block> GetBlockByIdAsync(int blockid)
        {
            var block = await _blockRepository.GetBlockByIdAsync(blockid);
            if(block == null)
            {
                throw new KeyNotFoundException($"Block with Id {blockid} not found");
            }
            return block;
        }
        public async Task<Block> UpdateBlockAsync(Block block)
        {
            var blockexisting = await _blockRepository.GetBlockByIdAsync(block.BlockId);
            if(blockexisting == null)
            {
                throw new KeyNotFoundException($"Block with Id {block.BlockId} not found");
            }
            blockexisting.BlockNumber = block.BlockNumber;
            blockexisting.SocietyId = block.SocietyId;
            blockexisting.IsActive = true;
            blockexisting.UpdateBy = 1;
            blockexisting.UpdateDate = DateTime.Now;

            await _blockRepository.UpdateBlockAsync(blockexisting);
            return block;
        }
        public async Task DeleteBlockByIdAsync(int blockid)
        {
            var blockexisting = await _blockRepository.GetBlockByIdAsync(blockid);
            if (blockexisting == null)
            {
                throw new KeyNotFoundException($"Block with Id {blockid} not found");
            }
            await _blockRepository.DeleteBlockAsync(blockexisting);
        }
        public async Task<List<Block>> GetBlocksBySocietyIdAsync(int societyId)
        {
            return await _blockRepository.GetBlocksBySocietyIdAsync(societyId);
        }
    }
}
