using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Application.Services
{
    public class FlatService : IFlatService
    {
        private readonly IFlatAdapter _flatAdapter;
        public FlatService(IFlatAdapter IFlatAdapter)
        {
            _flatAdapter = IFlatAdapter;
        }
        public async Task<IList<Flat>> GetFlatAsync()
        {
            return await _flatAdapter.GetFlatAsync();
        }
        public async Task<IList<Flat?>> GetFlatByIdAsync(int? societyId, int blockId)
        {
            return await _flatAdapter.GetFlatByIdAsync(societyId, blockId);
        }
        //public async Task<List<Flat>> AddFlatAsync(Flat flat)
        //{
        //    return await _flatAdapter.AddFlatAsync(flat);
        //}
        public async Task<List<Flat>> UpdateFlatAsync(Flat flat)
        {
            return await _flatAdapter.UpdateFlatAsync(flat);
        }
        public async Task<IList<Flat>> GetFlatByBlockId(int blockid)
        {
            var result = await _flatAdapter.GetFlatByBlockId(blockid);
            return result;
        }
        public async Task<IList<FlatWithMembersDto>> GetQR(int blockid, int eventId)
        {
            var result = await _flatAdapter.GetQR(blockid,eventId);
            return result;
        }
        public async Task<IList<FlatWithMembersDto>> GetGuestQR(int blockid, int EventId)
        {
            var result = await _flatAdapter.GetGuestQR(blockid, EventId);
            return result;
        }
    }
}

