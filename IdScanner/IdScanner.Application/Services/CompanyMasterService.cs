using IdScanner.Application.Interface;
using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Services
{
    public class CompanyMasterService : ICompanyMasterService
    {
        private readonly ICompanyMasterRepository _companyMasterRepository;
        public CompanyMasterService(ICompanyMasterRepository companyMasterRepository)
        {
            _companyMasterRepository = companyMasterRepository;
        }
        public async Task<CompanyMaster> CreateCompanyMasterAsync(CompanyMaster companyMaster)
        {
            var newCompany = new CompanyMaster
            {
                CompanyName = companyMaster.CompanyName,
                City = companyMaster.City,
                logo = companyMaster.logo,
                IsActive = true,
                InsertBy = 1,
                InsertDate = DateTime.Now,
                UpdateBy = 1,
                UpdateDate = DateTime.Now
            };
            return await _companyMasterRepository.AddCompanyMasterAsync(newCompany);
        }
        public async Task<List<CompanyMaster>> GetAllCompanyMasterAsync()
        {
            var companyMaster = await _companyMasterRepository.GetAllCompanyMaster();
            return companyMaster.Select(companyMaster => new CompanyMaster
            {
                CompanyId = companyMaster.CompanyId,
                CompanyName = companyMaster.CompanyName,
                City = companyMaster.City,
                logo = companyMaster.logo,
                IsActive = companyMaster.IsActive,
                InsertBy = companyMaster.InsertBy,
                InsertDate = companyMaster.InsertDate,
                UpdateBy = companyMaster.UpdateBy,
                UpdateDate = companyMaster.UpdateDate,

            }).ToList();
        }
        public async Task<CompanyMaster> UpdateCompanyMasterAsync(int menuid, CompanyMaster companyMaster)
        {

            var companyMasterExisting = await _companyMasterRepository.GetCompanyMasterById(menuid);

            if (companyMasterExisting == null)
            {
                throw new Exception($"UserRole with ID {menuid} not found.");
            }
            companyMasterExisting.CompanyName = companyMaster.CompanyName;
            companyMasterExisting.City = companyMaster.City;
            companyMasterExisting.logo = companyMaster.logo;
            companyMasterExisting.IsActive = true;
            companyMasterExisting.InsertBy = 1;
            companyMasterExisting.InsertDate = DateTime.UtcNow;
            companyMasterExisting.UpdateBy = 1;
            companyMasterExisting.UpdateDate = DateTime.UtcNow;


            await _companyMasterRepository.UpdateCompanyMasterAsync(companyMasterExisting);

            return companyMasterExisting;
        }
        public async Task DeleteCompanyMasterById(int companyid)
        {
            var deleteUserRole = await _companyMasterRepository.GetCompanyMasterById(companyid);
            if (deleteUserRole == null)
            {
                throw new KeyNotFoundException($"UserRole ID {companyid} not found.");
            }

            await _companyMasterRepository.DeleteCompanyMasterAsync(deleteUserRole);
        }
        public async Task<CompanyMaster> GetCompanyMasterById(int id)
        {
            var companyMaster = await _companyMasterRepository.GetCompanyMasterById(id);
            if (companyMaster == null)
            {
                throw new KeyNotFoundException($"Company Master with ID {id} not found.");
            }

            return companyMaster;
        }


    }
}
