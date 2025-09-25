using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Society> societies { get; set; }
        public DbSet<Block> blocks { get; set; }
        public DbSet<Flat> flats { get; set; }
        public DbSet<Member> members { get; set; }
        public DbSet<MenuMaster> MenuMasters { get; set; }
        public DbSet<SocietyData> SocietyData { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

    }
}
