using Teram.Framework.Core.Service;
using Teram.Web.Core.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Teram.DataAccessLayer
{
    public class TeramDbContext : TeramBaseContext
    {
        public TeramDbContext(DbContextOptions<TeramDbContext> options, IServiceProvider services, ILogger<TeramDbContext> logger) : base(options, services, logger)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            GlobalConfigurations.Assemblies.ForEach(x => modelBuilder.ApplyConfigurationsFromAssembly(x));
            GlobalConfigurations.Entities.Where(x => !x.IsAbstract).ToList().ForEach(x => modelBuilder.Entity(x));
            base.OnModelCreating(modelBuilder);
        }
    }
}
