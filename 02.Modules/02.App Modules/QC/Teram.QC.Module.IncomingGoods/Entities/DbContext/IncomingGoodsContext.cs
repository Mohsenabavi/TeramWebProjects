using Microsoft.EntityFrameworkCore;
using Teram.Framework.Core.Service;
using Teram.Web.Core.Configurations;

namespace Teram.QC.Module.IncomingGoods.Entities.DbContext
{
    public class IncomingGoodsContext :TeramBaseContext
    {
        private readonly string connectionString;

        public DbSet<ControlPlanCategory> ControlPlanCategories { get; set; }
        public DbSet<ControlPlan> ControlPlans { get; set; }
        public DbSet<IncomingGoodsInspection> IncomingGoodsInspections { get; set; }
        public DbSet<IncomingGoodsInspectionItem> IncomingGoodsInspectionsItems { get; set; }
        public IncomingGoodsContext()
        {
            connectionString = GlobalConfiguration.Configurations.ModuleDevelopeConnectionString;
        }

        public IncomingGoodsContext(IConfiguration configuration)
        {
            configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString("TeramConnectionString");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("_IncomingGoodsMigrationHistory"));
        }
    }
}
