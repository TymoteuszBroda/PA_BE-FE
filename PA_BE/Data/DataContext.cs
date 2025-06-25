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
        public DbSet<Licence> Licences {get; set;}
        public DbSet<EmployeeLicence> EmployeeLicences {get; set;}
        public DbSet<LicenceInstance> LicenceInstances {get; set;}
    }
}

