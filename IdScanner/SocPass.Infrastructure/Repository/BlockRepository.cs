using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;

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
        public async Task<List<Block>> GetBlockAsync()
        {
            return await _context.blocks.Where(x => x.IsActive == true && x.Society.IsActive == true).Include(e => e.Society).OrderBy(e => e.Society.Name).ThenBy(e => e.BlockNumber).ToListAsync();
        }
        public async Task<Block> GetBlockByIdAsync(int blockid)
        {
            return await _context.blocks.Include(e => e.Society)
                  .Where(x => x.IsActive == true).FirstAsync(e => e.BlockId == blockid);
        }
        public async Task UpdateBlockAsync(Block block)
        {
            _context.blocks.Update(block);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteBlockAsync(int blockid)
        {
            var existingBlock = await _context.blocks.FindAsync(blockid);
            if (existingBlock != null)
            {
                existingBlock.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Block>> GetBlocksBySocietyIdAsync(int societyId)
        {
            return await _context.blocks
                .Include(x => x.Society)
                .Where(d => d.SocietyId == societyId && d.IsActive)
                .OrderBy(x => x.BlockNumber)
                .ToListAsync();
        }
    }
}
