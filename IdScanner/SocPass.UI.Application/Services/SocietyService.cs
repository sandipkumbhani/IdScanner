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
    public class SocietyService:ISocietyService
    {
        private readonly ISocietyRepository _societyRepository;
        public SocietyService(ISocietyRepository societyRepository)
        {
            _societyRepository = societyRepository;
        }
        public async Task<List<Society>> GetAllSocietyAsync()
        {
            return await _societyRepository.GetAllSocietyAsync();
        }

        public async Task<Society> GetSocietyByIdAsync(int? societyId)
        {
            return await _societyRepository.GetSocietyByIdAsync(societyId);
        }

        public async Task<string> AddSocietyAsync(Society society)
        {
            return await _societyRepository.AddSocietyAsync(society);
        }

        public async Task<string> UpdateSocietyAsync(Society society)
        {
            return await _societyRepository.UpdateSocietyAsync(society);
        }
        public async Task<string> DeleteSocietyAsync(int societyId)
        {
            return await _societyRepository.DeleteSocietyAsync(societyId);
        }
    }
}
