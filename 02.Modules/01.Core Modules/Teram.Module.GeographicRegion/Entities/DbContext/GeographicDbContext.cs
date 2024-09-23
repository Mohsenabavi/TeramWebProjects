using Microsoft.EntityFrameworkCore;
using Teram.Framework.Core.Service;
using Teram.Web.Core.Configurations;

namespace Teram.Module.GeographicRegion.Entities.DbContext
{
    public class GeographicDbContext :TeramBaseContext
    {
        public DbSet<GeographicRegion> GeographicRegions { get; set; }

        private readonly string connectionString;
        public GeographicDbContext()
        {
            connectionString = GlobalConfiguration.Configurations.ModuleDevelopeConnectionString;
        }

        public GeographicDbContext(IConfiguration configuration)
        {
            configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString("TeramConnectionString");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("_GeographicRegionMigrationHistory"));
        }
    }
}
