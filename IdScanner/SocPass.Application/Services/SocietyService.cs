using SocPass.Application.Interface;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Services
{
    public class SocietyService : ISocietyService
    {
        private ISocietyRepository _societyRepository;
        public SocietyService(ISocietyRepository societyRepository)
        {
            _societyRepository = societyRepository;
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
        public async Task<List<Society>> GetAllSocietyAsync()
        {
            var newSociety = await _societyRepository.GetAllSocietyAsync();
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
        public async Task<Society> UpdateAsync(int societyId, Society society)
        {
            var societyExisting = await _societyRepository.GetByIdAsync(societyId);
            if (societyExisting == null)
            {
                throw new KeyNotFoundException("Society with Id {societyId} not found");
            }
            societyExisting.Name= society.Name;
            societyExisting.Address = society.Address;
            societyExisting.Email = society.Email;
            societyExisting.Contact = society.Contact;
            societyExisting.Contact2 = society.Contact2;
            societyExisting.IsActive = true;
            societyExisting.InsertBy = 1;
            societyExisting.InsertDate= DateTime.Now;
            societyExisting.UpdateBy = 1;
            societyExisting.UpdateDate= DateTime.Now;

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

            await _societyRepository.DeleteSocietyAsync(deleteSociety);
        }

    }
}
