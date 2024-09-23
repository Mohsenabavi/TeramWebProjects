using Hangfire;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using Teram.Framework.Core.Service;

namespace Teram.IT.Module.BackupManagement.Entities
{

    public class BackupDbContext : TeramBaseContext
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public DbSet<Application> Applications { get; set; }

        public DbSet<ServerPath> ServerPaths { get; set; }

        public DbSet<BackupHistory> BackupHistories { get; set; }

        public DbSet<JobRunHistory> JobRunHistories { get; set; }

        public BackupDbContext(IConfiguration configuration)
        {
            configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString("TeramConnectionString");
        }
        public BackupDbContext()
        {
            connectionString = GlobalConfiguration.Configurations.ModuleDevelopeConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("_BackupManagementMigrationHistory"));
        }
    }
}
