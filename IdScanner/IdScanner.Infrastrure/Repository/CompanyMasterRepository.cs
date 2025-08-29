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
    public class CompanyMasterRepository : ICompanyMasterRepository
    {
        private AppDbContext _context;
        public CompanyMasterRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CompanyMaster> AddCompanyMasterAsync(CompanyMaster companyMaster)
        {
            _context.CompanyMasters.Add(companyMaster);
            //_context.SaveChanges();
            await _context.SaveChangesAsync();
            return companyMaster;

        }
        public async Task<List<CompanyMaster>> GetAllCompanyMaster()
        {
            return await _context.CompanyMasters.Where(u => u.IsActive).ToListAsync();
        }
        public async Task<CompanyMaster> GetCompanyMasterById(int id)
        {
            return _context.CompanyMasters
                .FirstOrDefault(e => e.CompanyId == id);
        }
        public async Task DeleteCompanyMasterAsync(CompanyMaster companyMaster)
        {
            var existingCompany = await _context.CompanyMasters.FindAsync(companyMaster.CompanyId);
            if (existingCompany != null)
            {
                existingCompany.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateCompanyMasterAsync(CompanyMaster companyMaster)
        {
            _context.CompanyMasters.Update(companyMaster);
            _context.SaveChanges();
        }

    }
}
