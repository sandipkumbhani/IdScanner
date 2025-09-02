using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using IdScanner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Infrastructure.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Department> AddDepartmentAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }
        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.Include(x => x.CompanyMaster).Where(u => u.IsActive).ToListAsync();
        }
        public Department GetDepartmentById(int id)
        {
            return _context.Departments
                .Include(e => e.CompanyMaster)
                .FirstOrDefault(e => e.DepartmentId == id);
        }
        public async Task DeleteDepartmentAsync(Department department)
        {
            var existingDepartment = await _context.Departments.FindAsync(department.DepartmentId);
            if (existingDepartment != null)
            {
                existingDepartment.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateDepartmentAsync(Department department)
        {
            _context.Departments.Update(department);
            _context.SaveChanges();
        }
        public async Task<List<Department>> GetDepartmentsByCompanyIdAsync(int companyId)
        {
            return await _context.Departments
                .Where(d => d.CompanyId == companyId)
                .Select(d => new Department
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName
                })
                .ToListAsync();
        }

    }
}
