using Microsoft.EntityFrameworkCore;
using Teram.Framework.Core.Service;
using Teram.Web.Core.Configurations;

namespace Teram.IT.Module.Employee.Entities.DbContext
{
      public class HREmployeeContext : TeramBaseContext
    {
        private readonly string connectionString;

        public DbSet<HREmployee> Employees { get; set; }        

        public HREmployeeContext()
        {
            connectionString = GlobalConfiguration.Configurations.ModuleDevelopeConnectionString;
        }

        public HREmployeeContext(IConfiguration configuration)
        {
            configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString("TeramConnectionString");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("_EmployeeMigrationHistory"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
