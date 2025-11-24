using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Interfaces
{
    public interface IBlockAdapter
    {
        Task<IList<Block>> GetBlockAsync();
        Task<string> DeleteBlockAsync(int blockid);
        Task<string> AddBlockAsync(Block block);
        Task<Block> GetBlockByIdAsync(int? blockid);
        Task<string> UpdateBlockAsync(Block block);
        Task<IList<Block>> GetBlockBySocietyId(int? societyId);
    }
}
