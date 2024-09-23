using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using Teram.Framework.Core.Service;

namespace Teram.HR.Module.TicketRegister.Entities.DbContext
{

    public class TicketRegisterDbContext : TeramBaseContext
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public DbSet<Area> Areas { get; set; }
        public DbSet<AreaRow> AreaRows { get; set; }
        public DbSet<Seat> Seats { get; set; }        

        public TicketRegisterDbContext(IConfiguration configuration)
        {
            configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString("TeramConnectionString");
        }
        public TicketRegisterDbContext()
        {
            connectionString = GlobalConfiguration.Configurations.ModuleDevelopeConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("_TicketRegisterMigrationHistory"));
        }
    }
}
