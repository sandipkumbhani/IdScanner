using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;
using IdScanner.UI.Domain.Interfaces;

namespace IdScanner.UI.Application.Services
{
    public class DepartmentMasterService : IDepartmentMasterService
    {
        private readonly IDepartmentMasterRepository _departmentMasterRepository;
        public DepartmentMasterService(IDepartmentMasterRepository departmentMasterRepository)
        {
            _departmentMasterRepository = departmentMasterRepository;
        }

        public async Task<List<Department>> GetAllDepartmentMasterAsync()
        {
            return await _departmentMasterRepository.GetAllDepartmentAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int? id)
        {
            return await _departmentMasterRepository.GetDepartmentByIdAsync(id);
        }

        public async Task<string> AddDepartmentAsync(Department departmentMaster)
        {
            return await _departmentMasterRepository.AddDepartmentAsync(departmentMaster);
        }

        public async Task<string> UpdateDepartmentAsync(Department departmentMaster)
        {
            return await _departmentMasterRepository.UpdateDepartmentAsync(departmentMaster);
        }
        public async Task<string> DeleteDepartmentAsync(int id)
        {
            return await _departmentMasterRepository.DeleteDepartmentAsync(id);
        }
    }
}
