using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Interface
{
    public interface IDepartmentRepository
    {
        Task<Department> AddDepartmentAsync(Department department);
        Task<List<Department>> GetAllDepartmentsAsync();
        Department GetDepartmentById(int id);
        Task DeleteDepartmentAsync(Department department);
        Task UpdateDepartmentAsync(Department department);

    }
}
