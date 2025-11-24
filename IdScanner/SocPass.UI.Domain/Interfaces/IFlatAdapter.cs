using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Interfaces
{
    public interface IFlatAdapter
    {
        Task<IList<Flat>> GetFlatAsync();
        Task<IList<Flat?>> GetFlatByIdAsync(int? societyId, int blockId);
        //Task<List<Flat>> AddFlatAsync(Flat flat);
        Task<List<Flat>> UpdateFlatAsync(Flat flat);
        Task<IList<Flat>> GetFlatByBlockId(int blockid);
        Task<IList<FlatWithMembersDto>> GetQR(int blockid, int eventId);
        Task<IList<FlatWithMembersDto>> GetGuestQR(int blockid, int EventId);
    }
}
