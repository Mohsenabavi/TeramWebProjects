using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Entities.Map;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;
using Teram.Web.Core.Configurations;

namespace Teram.QC.Module.FinalProduct.Entities.DbContext
{
    public class FinalProductDbContext : TeramBaseContext
    {
        private readonly string connectionString;
        public DbSet<QCControlPlan> QCControlPlans { get; set; }
        public DbSet<QCDefect> QCDefects { get; set; }
        public DbSet<AcceptancePeriod> AcceptancePeriods { get; set; }
        public DbSet<FinalProductInspection> FinalProductInspections { get; set; }
        public DbSet<FlowInstruction> FlowInstructions { get; set; }
        public DbSet<FlowInstructionCondition> FlowInstructionConditions { get; set; }
        public DbSet<FinalProductNonComplianceCartableItem> FinalProductNonComplianceCartableItems { get; set; }
        public DbSet<Causation.Causation> Causations { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<RootCause> RootCauses { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<WorkStation> WorkStations { get; set; }
        public DbSet<CorrectiveAction> CorrectiveActions { get; set; }

        public FinalProductDbContext()
        {
            connectionString = GlobalConfiguration.Configurations.ModuleDevelopeConnectionString;
        }

        public FinalProductDbContext(IConfiguration configuration)
        {
            configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString("TeramConnectionString");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("_FinalProductMigrationHistory"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FinalProductInspectionConfiguration());
        }
    }
}
