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
        public async Task<IList<Society>> GetSocietyAsync()
        {
            return await _societyRepository.GetSocietyAsync();
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
