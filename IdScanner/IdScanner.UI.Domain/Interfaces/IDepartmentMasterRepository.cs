using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;

namespace IdScanner.UI.Domain.Interfaces
{
    public interface IDepartmentMasterRepository
    {
        Task<List<Department>> GetAllDepartmentAsync();
        Task<Department> GetDepartmentByIdAsync(int? id);
        Task<string> AddDepartmentAsync(Department departmentMaster);
        Task<string> UpdateDepartmentAsync(Department departmentMaster);
        Task<string> DeleteDepartmentAsync(int id);
        Task<List<Department>> GetDepartmentByCompanyId(int? companyId);
    }
}
