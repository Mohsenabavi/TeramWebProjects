using Microsoft.EntityFrameworkCore;
using Teram.Framework.Core.Service;

namespace Teram.Module.AttachmentsManagement.Entities.DbContext
{
    public class AttachmentContext :TeramBaseContext
    {

        private readonly string connectionString;

        public DbSet<Attachmant> Attachmants { get; set; }
        public AttachmentContext()
        {
            connectionString = GlobalConfiguration.Configurations.ModuleDevelopeConnectionString;
        }

        public AttachmentContext(IConfiguration configuration)
        {
            configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString("TeramConnectionString");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("_AttachmentMigrationHistory"));
        }

    }
}
