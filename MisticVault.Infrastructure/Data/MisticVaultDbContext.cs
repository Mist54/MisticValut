using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using MisticVault.Core.Todo.Entities;
using MisticVault.Infrastructure.Configuration;

namespace MisticVault.Infrastructure.Data
{
    public class MisticVaultDbContext : DbContext
    {
        public MisticVaultDbContext(DbContextOptions<MisticVaultDbContext> options) : base(options)
        {
        }

        public DbSet<MisticVault.Core.Todo.Entities.Todo> Todo => Set<MisticVault.Core.Todo.Entities.Todo>();
        public DbSet<TodoCategory> TodoCategory => Set<TodoCategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply configurations for Todo and TodoCategory entities
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MisticVaultDbContext).Assembly);
        }
    }
}
