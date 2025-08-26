using IdScanner.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MenuMaster> MenuMasters { get; set; }
        public DbSet<CompanyMaster> CompanyMasters { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserData> UserDatas { get; set; }
    }
}
