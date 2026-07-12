using Microsoft.EntityFrameworkCore;
using MisticVault.Core.Common.Entities;
using MisticVault.Core.Todo.Entities;

namespace MisticVault.Infrastructure.Data
{
    public class MisticVaultDbContext : DbContext
    {
        public MisticVaultDbContext(DbContextOptions<MisticVaultDbContext> options) : base(options)
        {
        }

        //Setting up the tables for the entities
        public DbSet<Todo> Todos => Set<Todo>();
        public DbSet<TodoCategory> TodoCategories => Set<TodoCategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply configurations for Todo and TodoCategory entities
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MisticVaultDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        // 2. Intercept Sync Saves (Just in case you use them)
        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }
        private void ApplyAuditInfo()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var now = DateTime.UtcNow; // Using UTC is highly recommended for databases

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    // Generate Guid if it hasn't been set yet
                    if (entry.Entity.Id == Guid.Empty)
                    {
                        entry.Entity.Id = Guid.NewGuid();
                    }
                    entry.Entity.CreatedDate = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDate = now;

                    // Prevent accidental overwriting of CreatedDate during updates
                    entry.Property(x => x.CreatedDate).IsModified = false;
                }
            }
        }
    }
}
