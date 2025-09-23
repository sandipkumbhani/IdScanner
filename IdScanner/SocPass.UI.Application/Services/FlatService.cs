using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Application.Services
{
    public class FlatService : IFlatService
    {
        private readonly IFlatRepository _FlatRepository;
        public FlatService(IFlatRepository flatRepository)
        {
            _FlatRepository = flatRepository;
        }
        public async Task<List<Flat>> GetAllFlatAsync()
        {
            return await _FlatRepository.GetAllFlatAsync();
        }
        public async Task<List<Flat?>> GetFlatByIdAsync(int? societyId, int blockId)
        {
            return await _FlatRepository.GetFlatByIdAsync(societyId, blockId);
        }
        public async Task<List<Flat>> AddFlatAsync(Flat flat)
        {
            return await _FlatRepository.AddFlatAsync(flat);
        }
        public async Task<List<Flat>> UpdateFlatAsync(Flat flat)
        {
            return await _FlatRepository.UpdateFlatAsync(flat);
        }
        public async Task<List<Flat>> GetFlatByBlockId(int blockid)
        {
            var result = await _FlatRepository.GetFlatByBlockId(blockid);
            return result;
        }
        public async Task<List<Flat>> GetQR(int blockid)
        {
            var result = await _FlatRepository.GetQR(blockid);
            return result;
        }
        public async Task<List<Flat>> GetGuestQR(int blockid)
        {
            var result = await _FlatRepository.GetGuestQR(blockid);
            return result;
        }
    }
}

