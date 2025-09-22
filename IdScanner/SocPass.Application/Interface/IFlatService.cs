

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;

namespace SocPass.Application.Interface
{
    public interface IFlatService
    {
        Task<List<Flat>> CreateFlatAsync(Flat flat);
        Task<List<Flat>> GetAllFlatAsync();
        Task<Flat> GetByIdAsync(int flatid);
        //Task<List<Flat>> UpdateAsync(int societyId, int blockId, Flat flat);
        Task<List<Flat>> UpdateFlatAsync(Flat flat);
        Task<List<Flat>> GetFlatByBlockID(int blockid);
        Task<List<FlatWithMembersDto>> getqr(int blockid);

    }
}
