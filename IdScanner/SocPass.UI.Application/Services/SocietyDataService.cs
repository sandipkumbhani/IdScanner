using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.DTO;
using SocPass.Domain.Model; 
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;


namespace SocPass.UI.Application.Services
{
    public class SocietyDataService : ISocietyDataService
    {
        private readonly ISocietyDataAdapter _societyDataRepository;
        public SocietyDataService(ISocietyDataAdapter societyDataRepository)
        {
            _societyDataRepository = societyDataRepository;
        }

        public async Task<IList<SocietyData>> GetAllSocietyDataAsync()
        {
            return await _societyDataRepository.GetAllSocietyData();
        }

        public async Task<SocietyData> GetSocietyDataByIdAsync(int? societyDataId)
        {
            return await _societyDataRepository.GetSocietyDataByIdAsync(societyDataId);
        }
        public async Task<string> AddSocietyDataAsync(SocietyDataCreateRequest societyData)
        {
            return await _societyDataRepository.AddSocietyDataAsync(societyData);
        }
        public async Task<string> UpdateSocietyDataAsync(SocietyData societyData)
        {
            return await _societyDataRepository.UpdateSocietyDataAsync(societyData);
        }
        public async Task<string> DeleteSocietyDataAsync(int societyDataId)
        {
            return await _societyDataRepository.DeleteSocietyDataAsync(societyDataId);
        }
        //public async Task<List<SocietyData>> GetSocietyDataByFlatId(int flatId)
        //{
        //    return await _societyDataRepository.GetSocietyDataByFlatId(flatId);
        //}

    }
}
