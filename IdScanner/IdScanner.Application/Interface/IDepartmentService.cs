using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Interface
{
    public interface IDepartmentService
    {
        Task<Department> CreateDepartmentAsync(Department department);
        Department GetDepartmentDetailsById(int departId);
        Task<Department> UpdateDepartmentAsync(int departmentId, Department department);
        Task<List<Department>> GetAllDepartmentAsync();
        Task DeleteDepaetmentById(int id);
    }
}
