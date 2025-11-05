using SocPass.Domain.DTO;
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
        Task<List<FlatWithMembersDto>> GetQR(int blockid, int eventId);
        Task<List<FlatWithMembersDto>> GetGuestQR(int blockid, int EventId);
    }
}
