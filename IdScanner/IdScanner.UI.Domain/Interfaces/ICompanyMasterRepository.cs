using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;

namespace IdScanner.UI.Domain.Interfaces
{
    public interface ICompanyMasterRepository
    {
        Task<List<CompanyMaster>> GetAllCompanyAsync();
        Task<CompanyMaster> GetCompanyByIdAsync(int? id);
        Task<string> AddCompanyAsync(CompanyMaster companyMaster);
        Task<string> UpdateCompanyAsync(CompanyMaster companyMaster);
        Task<string> DeleteCompanyAsync(int id);

    }
}
