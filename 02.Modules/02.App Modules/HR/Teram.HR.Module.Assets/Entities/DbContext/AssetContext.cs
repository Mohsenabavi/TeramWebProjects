using Hangfire;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using Teram.Framework.Core.Service;

namespace Teram.HR.Module.Assets.Entities.DbContext
{
    public class AssetContext : TeramBaseContext
    {
        private readonly string connectionString;

        public DbSet<RahkaranAsset> RahkaranAssets { get; set; }    

        public DbSet<AssetSelfExpression> AssetSelfExpressions { get; set; }

        public AssetContext()
        {
            connectionString = GlobalConfiguration.Configurations.ModuleDevelopeConnectionString;
        }

        public AssetContext(IConfiguration configuration)
        {
            configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString("TeramConnectionString");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("_AssetsMigrationHistory"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {         
            base.OnModelCreating(modelBuilder);
        }
    }
}
