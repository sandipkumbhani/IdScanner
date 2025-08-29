using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;

namespace IdScanner.UI.Application.Interface
{
    public interface ICompanyMasterService
    {
        Task<List<CompanyMaster>> GetAllCompanyMasterAsync();
        Task<CompanyMaster> GetCompanyByIdAsync(int? id);
        Task<string> AddCompanyAsync(CompanyMaster companyMaster);
        Task<string> UpdateCompanyAsync(CompanyMaster companyMaster);
        Task<string> DeleteCompanyAsync(int id);
    }
}
