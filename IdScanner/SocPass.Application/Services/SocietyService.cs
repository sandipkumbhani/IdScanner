using Microsoft.AspNetCore.Http;
using SocPass.Application.Interface;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;

namespace SocPass.Application.Services
{
    public class SocietyService : ISocietyService
    {
        private ISocietyRepository _societyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SocietyService(ISocietyRepository societyRepository, IHttpContextAccessor httpContextAccessor)
        {
            _societyRepository = societyRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Society> CreateSocietyAsync(Society society)
        {
            var newSociety = new Society
            {
                Name = society.Name,
                Address = society.Address,
                Email = society.Email,
                Contact = society.Contact,
                Contact2 = society.Contact2,
                IsActive = true,
                InsertBy = 1,
                InsertDate = DateTime.Now,
                UpdateBy = 1,
                UpdateDate = DateTime.Now

            };
            return await _societyRepository.CreateSocietyAsync(newSociety);
        }
        public async Task<List<Society>> GetSocietyAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var newSociety = await _societyRepository.GetSocietyAsync(userId);
            return newSociety ?? new List<Society>();
        }

        public async Task<List<Society>> GetSocietyUserIdAsync(int userId)
        {
            var newSociety = await _societyRepository.GetSocietyAsync(userId);
            return newSociety ?? new List<Society>();
        }

        public async Task<Society> GetById(int societyId)
        {
            var society = await _societyRepository.GetByIdAsync(societyId);
            if (society == null)
            {
                throw new KeyNotFoundException($"Society with Id {societyId} not found");
            }
            return society;
        }
        public async Task<Society> UpdateAsync(Society society)
        {
            var societyExisting = await _societyRepository.GetByIdAsync(society.SocietyId);
            if (societyExisting == null)
            {
                throw new KeyNotFoundException("Society with Id {societyId} not found");
            }
            societyExisting.Name = society.Name;
            societyExisting.Address = society.Address;
            societyExisting.Email = society.Email;
            societyExisting.Contact = society.Contact;
            societyExisting.Contact2 = society.Contact2;
            societyExisting.IsActive = true;
            societyExisting.InsertBy = 1;
            societyExisting.InsertDate = DateTime.Now;
            societyExisting.UpdateBy = 1;
            societyExisting.UpdateDate = DateTime.Now;

            await _societyRepository.UpdateSocietyAsync(societyExisting);
            return society;
        }
        public async Task DeleteSocietyById(int society)
        {
            var deleteSociety = await _societyRepository.GetByIdAsync(society);
            if (deleteSociety == null)
            {
                throw new KeyNotFoundException($"Society ID {society} not found.");
            }

            await _societyRepository.DeleteSocietyAsync(society);
        }

    }
}
