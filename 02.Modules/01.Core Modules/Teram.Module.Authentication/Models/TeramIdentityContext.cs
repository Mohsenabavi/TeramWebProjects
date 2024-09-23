using Teram.Module.Authentication.Entities;
using Teram.Module.Authentication.Logic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Teram.Module.Authentication.Models
{
    public class TeramIdentityContext : IdentityDbContext<TeramUser, TeramRole, Guid>, IIdentityUnitOfWork
    {
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenParameter> TokenParameters { get; set; }

        public Task BulkMerge<TEntity>(List<TEntity> entities, CancellationToken cancellationToken = new CancellationToken()) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task BulkMerge<TEntity>(List<TEntity> entities, bool hasDeleteOperation = false,
            CancellationToken cancellationToken = new CancellationToken()) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public DbContext Context => this;

        public TeramIdentityContext(DbContextOptions<TeramIdentityContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Token>().HasOne(x => x.Issuer).WithMany().HasForeignKey(x => x.IssuerId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Token>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict); 
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void RejectChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Modified:
                        break;
                }
            }
        }



        public int SaveChanges(bool validateOnSaveEnabled, bool autoDetect = true, bool invalidateCacheDependencies = false)
        {
            try
            {
                var result = base.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void SaveChangesAsync(bool validateOnSaveEnabled, bool invalidateCacheDependencies = false)
        {
            try
            {
                base.SaveChangesAsync();
                if (invalidateCacheDependencies)
                {
                    //new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int SqlQuery<TRow>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public int ExecuteStoredProcedure(string spName, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteStoredProcedure<T>(string spName, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> ExecuteEntityStoredProcedure<TEntity>(string spName, object[] parameters) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public TResult ExecuteRowEntityStoredProcedure<TEntity, TResult>(string spName, object[] parameters) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task BulkInsert<TEntity>(List<TEntity> entities, CancellationToken cancellationToken = new CancellationToken()) where TEntity : class
        {
            throw new NotImplementedException();
        }

		public Task BulkMerge<TEntity>(List<TEntity> entities, bool hasDeleteOperation = false, List<string> compareColumnNames = null, List<string> updateColumnNames = null, List<string> insertColumnsNames = null, CancellationToken cancellationToken = default) where TEntity : class
		{
			throw new NotImplementedException();
		}
	}
}
