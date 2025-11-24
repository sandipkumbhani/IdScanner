using Microsoft.AspNetCore.Http;
using SocPass.Application.Interface;
using SocPass.Domain.DTO;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System.Security.Claims;

namespace SocPass.Application.Services
{
    public class SocietyDataService : ISocietyDataService
    {
        private readonly ISocietyDataRepository _societyDataRepository;
        private readonly IFlatRepository _flatRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISocietyRepository _societyRepository;

        public SocietyDataService(ISocietyDataRepository societyDataRepository, IFlatRepository flatRepository,
            IHttpContextAccessor httpContextAccessor, ISocietyRepository societyRepository)
        {
            _societyDataRepository = societyDataRepository;
            _flatRepository = flatRepository;
            _httpContextAccessor = httpContextAccessor;
            _societyRepository = societyRepository;
        }
        public async Task<List<SocietyData>> GetSocietyDataAsync()
        {
            var role = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var SocietyDataList = new List<SocietyData>();
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                SocietyDataList = await _societyDataRepository.GetSocietyDataAsync();
            }
            else
            {
                var societies = await _societyRepository.GetSocietyAsync(userId);
                var society = societies.FirstOrDefault();
                if (society != null)
                {
                    SocietyDataList = (await _societyDataRepository.GetSocietyDataAsync())
                                 .Where(e => e.Flat?.SocietyId == society.SocietyId)
                                 .ToList();
                }
                else
                {
                    SocietyDataList = new List<SocietyData>();
                }
            }
            return SocietyDataList ?? new List<SocietyData>();
        }


        public async Task CreateSocietyDataAsync(SocietyDataCreateRequest request)
        {
            var existingSocietyData = await _societyDataRepository.GetSocietyDataByFlatId(request.FlatId);

            var checkExistingSocietyData = existingSocietyData
                .FirstOrDefault(b => b.Flat != null &&
                                     b.FlatId == request.FlatId);

            if (checkExistingSocietyData != null)
            {
                throw new InvalidOperationException($"Soicety Data Of Flat  already exists.");
            }


            // Get the maximum length among the lists
            int count = Math.Max(request.ContactName.Count,
                                 Math.Max(request.ContactNumber.Count, request.ContactEmail.Count));

            for (int i = 0; i < count; i++)
            {
                var newSocietyData = new SocietyData
                {
                    FlatId = request.FlatId,
                    ContactName = request.ContactName.ElementAtOrDefault(i), 
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

        public async Task<SocietyData> UpdateSocietyDataAsync(SocietyData societyData)
        {
            var existingData = await _societyDataRepository.GetSocietyDataByIdAsync(societyData.SocietyDataId);
            if (existingData == null)
                throw new Exception("SocietyData record not found.");

            existingData.ContactName = societyData.ContactName;
            existingData.ContactNumber = societyData.ContactNumber;
            existingData.ContactEmail = societyData.ContactEmail;
            existingData.FlatId = societyData.FlatId;
            existingData.IsActive = true;
            existingData.UpdateBy = 1;
            existingData.UpdateDate = DateTime.Now;

            await _societyDataRepository.UpdateSocietyAsync(existingData);
            return existingData;
        }
        public async Task<SocietyData>  GetSocietyDataByIdAsync(int societyDataId)
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
