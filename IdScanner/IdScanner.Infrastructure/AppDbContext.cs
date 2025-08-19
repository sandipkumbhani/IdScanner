using IdScanner.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContext) : base(dbContext)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        
    }
}
