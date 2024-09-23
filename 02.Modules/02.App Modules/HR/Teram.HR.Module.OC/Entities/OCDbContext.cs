using Microsoft.EntityFrameworkCore;
using Teram.Framework.Core.Service;
using Teram.Web.Core.Configurations;

namespace Teram.HR.Module.OC.Entities
{
    public class OCDbContext : TeramBaseContext
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public DbSet<OrganizationChart> OrganizationCharts { get; set; }
        public DbSet<Position> Positions { get; set; }

        public OCDbContext(IConfiguration configuration)
        {
            configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString("TeramConnectionString");
        }

        public OCDbContext()
        {
            connectionString = GlobalConfiguration.Configurations.ModuleDevelopeConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("_OCMigrationHistory"));
        }
    }
}
