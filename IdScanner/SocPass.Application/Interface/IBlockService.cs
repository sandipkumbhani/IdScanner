using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.Model;

namespace SocPass.Application.Interface
{
    public interface IBlockService
    {
        Task<Block> CreateBlockAsync(Block block);
        Task<List<Block>> GetBlockAsync();
        Task<Block> GetBlockByIdAsync(int blockid);
        Task<Block> UpdateBlockAsync(Block block);
        Task DeleteBlockByIdAsync(int blockid);
        Task<List<Block>> GetBlocksBySocietyIdAsync(int societyId);
    }
}
