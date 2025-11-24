using SocPass.Domain.DTO;
using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IFlatService
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
