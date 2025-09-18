using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Application.Interface
{
    public interface IBlockService
    {
        Task<List<Block>> GetAllBlockAsync();
        Task<Block?> GetBlockByIdAsync(int blockid);
        Task<string> DeleteBlockAsync(int blockid);
        Task<string> UpdateBlockAsync(Block block);
        Task<string> AddBlockAsync(Block block);
        Task<List<Block>> GetBlockBySocietyId(int? societyId);
    }
}
