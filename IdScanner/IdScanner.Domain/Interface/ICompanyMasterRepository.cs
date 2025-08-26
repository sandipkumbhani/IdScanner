using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Interface
{
    public interface ICompanyMasterRepository
    {
        Task<CompanyMaster> AddCompanyMasterAsync(CompanyMaster companyMaster);
        Task<List<CompanyMaster>> GetAllCompanyMaster();
        Task<CompanyMaster> GetCompanyMasterById(int id);
        Task DeleteCompanyMasterAsync(CompanyMaster companyMaster);
        Task UpdateCompanyMasterAsync(CompanyMaster companyMaster);

    }
}
