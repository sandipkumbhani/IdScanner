using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.UI.Domain.Interfaces;
using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;

namespace IdScanner.UI.Application.Services
{
    public class CompanyMasterService : ICompanyMasterService
    {
        private readonly ICompanyMasterRepository _companyMasterRepository;
        public CompanyMasterService(ICompanyMasterRepository companyMasterRepository)
        {
            _companyMasterRepository = companyMasterRepository;
        }

        public async Task<List<CompanyMaster>>GetAllCompanyMasterAsync()
        {
            return await _companyMasterRepository.GetAllCompanyAsync();
        }

        public async Task<CompanyMaster> GetCompanyByIdAsync(int? id)
        {
            return await _companyMasterRepository.GetCompanyByIdAsync(id);
        }

        public async Task<string> AddCompanyAsync(CompanyMaster companyMaster)
        {
            return await _companyMasterRepository.AddCompanyAsync(companyMaster);
        }

        public async Task<string> UpdateCompanyAsync(CompanyMaster companyMaster)
        {
            return await _companyMasterRepository.UpdateCompanyAsync(companyMaster);
        }
        public async Task<string> DeleteCompanyAsync(int id)
        {
            return await _companyMasterRepository.DeleteCompanyAsync(id);
        }
    }
}
