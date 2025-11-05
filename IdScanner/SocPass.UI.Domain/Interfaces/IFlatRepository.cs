using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Interfaces
{
    public interface IFlatRepository
    {
        Task<List<Flat>> GetAllFlatAsync();
        Task<List<Flat?>> GetFlatByIdAsync(int? societyId, int blockId);
        Task<List<Flat>> AddFlatAsync(Flat flat);
        Task<List<Flat>> UpdateFlatAsync(Flat flat);
        Task<List<Flat>> GetFlatByBlockId(int blockid);
        Task<List<FlatWithMembersDto>> GetQR(int blockid, int eventId);
        Task<List<FlatWithMembersDto>> GetGuestQR(int blockid, int EventId);
    }
}
