using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using Teram.Framework.Core.Service;

namespace Teram.Module.SmsSender.Entities.DbContext
{
    public class SMSSenderContext : TeramBaseContext
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public DbSet<SMSHistory> SMSHistories { get; set; }

        public DbSet<SMSTemplate> SMSTemplates { get; set; }

        public SMSSenderContext(IConfiguration configuration)
        {
            configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString("TeramConnectionString");
        }
        public SMSSenderContext()
        {
            connectionString = GlobalConfiguration.Configurations.ModuleDevelopeConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("_SMSSenderMigrationHistory"));
        }
    }
}
