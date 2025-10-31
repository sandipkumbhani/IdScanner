using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Interface
{
    public interface IFlatRepository
    {
        Task<Flat> CreateFlatAsync(Flat flat);
        Task<List<Flat>> GetAllFlatAsync();
        Task<Flat> GetById(int flatId);
        //Task UpdateAsync(Flat flat);
        Task<Flat?> GetFlatByPositionAsync(int societyId, int blockId);
        Task<List<Flat>> UpdateRangeAsync(List<Flat> flats);
        Task<Flat> UpdateAsync(Flat flat);
        Task<Flat> UpdateFlatAsync(Flat flat);
        Task<Flat> GetFlatByNumberAsync(int societyId, int blockId, int flatNumber);
        Task DeleteFlatAsync(int flatId);
        Task<List<Flat>> GetFlatsByBlockAsync(int societyId, int blockId);
        Task<List<Flat>> GetFlatByBlockIdAsync(int blockid);
        //Task<List<FlatWithMembersDto>> GetMemberQr(int blockid);
        Task<List<FlatWithMembersDto>> GetGuestQr(int blockid);

        Task<List<FlatWithMembersDto>> GetMemberQrAsync(int blockid, int eventId);
    }
}
