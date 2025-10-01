using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SocPass.Application.Interface;
using SocPass.Domain.DTO;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;

namespace SocPass.Application.Services
{
    public class SocietyDataService : ISocietyDataService
    {
        private readonly ISocietyDataRepository _societyDataRepository;
        private readonly IFlatRepository _flatRepository;

        public SocietyDataService(ISocietyDataRepository societyDataRepository, IFlatRepository flatRepository)
        {
            _societyDataRepository = societyDataRepository;
            _flatRepository = flatRepository;
        }

        public async Task<List<SocietyData>> GetAllSocietyDataAsync()
        {
            var societyDataList =  await _societyDataRepository.GetAllSocietyDataAsync();
            return societyDataList ?? new List<SocietyData>();

        }

        public async Task CreateSocietyDataAsync(SocietyDataCreateRequest request)
        {
            // Get the maximum length among the lists
            int count = Math.Max(request.ContactName.Count,
                                 Math.Max(request.ContactNumber.Count, request.ContactEmail.Count));

            for (int i = 0; i < count; i++)
            {
                var newSocietyData = new SocietyData
                {
                    FlatId = request.FlatId,
                    ContactName = request.ContactName.ElementAtOrDefault(i), // First element in first row, etc.
                    ContactNumber = request.ContactNumber.ElementAtOrDefault(i),
                    ContactEmail = request.ContactEmail.ElementAtOrDefault(i),
                    IsActive = true,
                    InsertBy = 1,
                    InsertDate = DateTime.Now,
                    UpdateBy = 1,
                    UpdateDate = DateTime.Now
                };

                await _societyDataRepository.AddSocietyDataAsync(newSocietyData);
            }
        }

        public async Task<SocietyData> UpdateSocietyDataAsync(int societyDataId, SocietyData request)
        {
            var existingData = await _societyDataRepository.GetSocietyDataByIdAsync(societyDataId);
            if (existingData == null)
                throw new Exception("SocietyData record not found.");

            // Pick the first element from the lists, if available
            //existingData.ContactName = request.ContactName.ElementAtOrDefault(0) ?? existingData.ContactName;
            //existingData.ContactNumber = request.ContactNumber.ElementAtOrDefault(0) ?? existingData.ContactNumber;
            //existingData.ContactEmail = request.ContactEmail.ElementAtOrDefault(0) ?? existingData.ContactEmail;
            //existingData.FlatId = request.FlatId;
            //existingData.IsActive = true;
            //existingData.UpdateBy = 1;
            //existingData.UpdateDate = DateTime.Now;

            existingData.ContactName = request.ContactName;
            existingData.ContactNumber = request.ContactNumber;
            existingData.ContactEmail = request.ContactEmail;
            existingData.FlatId = request.FlatId;
            existingData.IsActive = true;
            existingData.UpdateBy = 1;
            existingData.UpdateDate = DateTime.Now;

            await _societyDataRepository.UpdateSocietyAsync(existingData);
            return existingData;
        }
        public async Task<SocietyData> GetSocietyDataByIdAsync(int societyDataId)
        {
            var societyData = await _societyDataRepository.GetSocietyDataByIdAsync(societyDataId);
            if (societyData == null)
            {
                throw new KeyNotFoundException($"Society Data with ID {societyDataId} not found.");
            }

            return societyData;
        }

        public async Task DeleteSocietyDataByIdAsync(int societyDataId)
        {
            var deleteData = await _societyDataRepository.GetSocietyDataByIdAsync(societyDataId);
            if (deleteData == null)
            {
                throw new KeyNotFoundException($"Society Data with ID {societyDataId} not found.");
            }

            await _societyDataRepository.DeleteSocietyAsync(societyDataId);
        }

        public async Task<List<SocietyData>> GetSocietyDataByFlatId(int flatId)
        {
            var societyData = await _societyDataRepository.GetSocietyDataByFlatId(flatId);
            if (societyData == null)
            {
                throw new KeyNotFoundException($"Society Data with ID {flatId} not found.");
            }

            return societyData;
        }
    }
}
