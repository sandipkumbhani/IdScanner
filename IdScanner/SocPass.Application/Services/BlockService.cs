using SocPass.Application.Interface;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SocPass.Application.Services
{
    public class BlockService : IBlockService
    {
        private readonly IBlockRepository _blockRepository;
        public BlockService(IBlockRepository blockRepository)
        {
            _blockRepository = blockRepository;
        }

        public async Task<Block> CreateBlockAsync(Block block)
        {
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
            var newBlock = await _blockRepository.GetAllBlockAsync();
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

        public async Task<Block> UpdateBlockAsync(int blockid,Block block)
        {
            var blockexisting = await _blockRepository.GetBlockByIdAsync(blockid);
            if(blockexisting == null)
            {
                throw new KeyNotFoundException($"Block with Id {blockid} not found");
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
    }
}
