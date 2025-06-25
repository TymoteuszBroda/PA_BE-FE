using PermAdminAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PermAdminAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Employee> Employees {get; set;}
        public DbSet<License> Licenses {get; set;}
        public DbSet<EmployeeLicense> EmployeeLicenses {get; set;}
        public DbSet<LicenseInstance> LicenseInstances {get; set;}
    }
}

