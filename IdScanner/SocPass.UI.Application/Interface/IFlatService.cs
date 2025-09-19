using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IFlatService
    {
        Task<List<Flat>> GetAllFlatAsync();
        Task<List<Flat?>> GetFlatByIdAsync(int? societyId, int blockId);
        Task<List<Flat>> AddFlatAsync(Flat flat);
        Task<List<Flat>> UpdateFlatAsync(Flat flat);
        Task<List<Flat>> GetFlatByBlockId(int blockid);
        Task<List<Flat>> GetQR(int blockid);
    }
}
