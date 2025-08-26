using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Interface
{
    public interface ICompanyMasterService
    {
        Task<CompanyMaster> CreateCompanyMasterAsync(CompanyMaster companyMaster);
        Task<List<CompanyMaster>> GetAllCompanyMasterAsync();
        Task<CompanyMaster> UpdateCompanyMasterAsync(int menuid, CompanyMaster companyMaster);
        Task DeleteCompanyMasterById(int companyid);
        Task<CompanyMaster> GetCompanyMasterById(int id);
    }
}
