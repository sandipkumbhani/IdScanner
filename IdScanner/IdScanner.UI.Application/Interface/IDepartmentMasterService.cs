using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;

namespace IdScanner.UI.Application.Interface
{
    public interface IDepartmentMasterService
    {
        Task<List<Department>> GetAllDepartmentMasterAsync();
        Task<Department> GetDepartmentByIdAsync(int? id);
        Task<string> AddDepartmentAsync(Department departmentMaster);
        Task<string> UpdateDepartmentAsync(Department departmentMaster);
        Task<string> DeleteDepartmentAsync(int id);
        Task<List<Department>> GetDepartmentByCompanyId(int? companyId);
    }
}
