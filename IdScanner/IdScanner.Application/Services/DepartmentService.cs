using IdScanner.Application.Interface;
using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<List<Department>> GetAllDepartmentAsync()
        {
            var department = await _departmentRepository.GetAllDepartmentsAsync();

            return department.Select(departments => new Department
            {
                DepartmentId = departments.DepartmentId,
                DepartmentName = departments.DepartmentName,
                City = departments.City,
                CompanyId = departments.CompanyId,
                CompanyMaster = departments.CompanyMaster,
                IsActive = departments.IsActive,
                InsertBy = departments.InsertBy,
                InsertDate = departments.InsertDate,
                UpdateBy = departments.UpdateBy,
                UpdateDate = departments.UpdateDate,


            }).ToList();
        }
        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            if (department == null)
            {
                return null;
            }
            var newDepartment = new Department
            {
                DepartmentName = department.DepartmentName,
                City = department.City,
                CompanyId = department.CompanyId,
                IsActive = true,
                InsertBy = 1,
                InsertDate = DateTime.Now,
                UpdateBy = 1,
                UpdateDate = DateTime.Now
            };
            return await _departmentRepository.AddDepartmentAsync(newDepartment);
        }
        public Department GetDepartmentDetailsById(int departId)
        {
            var DepartmentDetails = _departmentRepository.GetDepartmentById(departId);
            if (DepartmentDetails == null)
            {
                throw new KeyNotFoundException($"User Id with ID {departId} not found.");
            }

            return new Department
            {
                DepartmentId = DepartmentDetails.DepartmentId,
                DepartmentName = DepartmentDetails.DepartmentName,
                City = DepartmentDetails.City,
                CompanyId = DepartmentDetails.CompanyId,
                CompanyMaster = DepartmentDetails.CompanyMaster,
                IsActive = DepartmentDetails.IsActive,
                InsertBy = DepartmentDetails.InsertBy,
                InsertDate = DepartmentDetails.InsertDate,
                UpdateBy = DepartmentDetails.UpdateBy,
                UpdateDate = DepartmentDetails.UpdateDate,

            };
        }
        public async Task<Department> UpdateDepartmentAsync(int departmentId, Department department)
        {

            var departmentExisting = _departmentRepository.GetDepartmentById(departmentId);

            if (departmentExisting == null)
            {
                throw new Exception($"User with ID {departmentId} not found.");
            }
            departmentExisting.DepartmentId = department.DepartmentId;
            departmentExisting.DepartmentName = department.DepartmentName;
            departmentExisting.City = department.City;
            departmentExisting.CompanyId = department.CompanyId;
            departmentExisting.IsActive = true;
            departmentExisting.InsertBy = 1;
            departmentExisting.InsertDate = DateTime.UtcNow;
            departmentExisting.UpdateBy = 1;
            departmentExisting.UpdateDate = DateTime.UtcNow;


            await _departmentRepository.UpdateDepartmentAsync(departmentExisting);

            return departmentExisting;
        }
        public async Task DeleteDepaetmentById(int id)
        {
            var deleteUser = _departmentRepository.GetDepartmentById(id);
            if (deleteUser == null)
            {
                throw new KeyNotFoundException($"User ID {id} not found.");
            }

            await _departmentRepository.DeleteDepartmentAsync(deleteUser);
        }
        public async Task<List<Department>> GetDepartmentsByCompanyIdAsync(int companyId)
        {
            return await _departmentRepository.GetDepartmentsByCompanyIdAsync(companyId);
        }

    }
}
