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
        private readonly ISocietyAdapter _societyRepository;
        public SocietyService(ISocietyAdapter societyRepository)
        {
            _societyRepository = societyRepository;
        }
        public async Task<IList<Society>> GetSocietyByUserId(int userId)
        {
            return await _societyRepository.GetSocietyByUserId(userId);
        }

        public async Task<IList<Society>> GetAllSocietyAsync()
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
