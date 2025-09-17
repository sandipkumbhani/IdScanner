using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.Model;

namespace SocPass.Domain.Interface
{
    public interface IBlockRepository
    {
        Task<Block> CreateBlockAsync(Block block);
        Task<List<Block>> GetAllBlockAsync();
        Task<Block> GetBlockByIdAsync(int blockid);
        Task UpdateBlockAsync(Block block);
        Task DeleteBlockAsync(Block block);

    }
}
