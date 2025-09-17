using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Infrastructure.Repository
{
    public class BlockRepository : IBlockRepository
    {
        private readonly AppDbContext _context;
        public BlockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Block> CreateBlockAsync(Block block)
        {
            _context.blocks.Add(block);
            await _context.SaveChangesAsync();
            return block;
        }

        public async Task<List<Block>> GetAllBlockAsync()
        {
            return await _context.blocks.Include(e => e.Society).ToListAsync();
        }

        public async Task<Block> GetBlockByIdAsync(int blockid)
        {
            return await _context.blocks.Include(e => e.Society).FirstOrDefaultAsync(e => e.BlockId == blockid);
        }

        public async Task UpdateBlockAsync(Block block)
        {
            _context.blocks.Update(block);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBlockAsync(Block block)
        {
            var existingBlock = await _context.blocks.FindAsync(block.BlockId);
            if (existingBlock != null)
            {
                existingBlock.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
       
    }
}
