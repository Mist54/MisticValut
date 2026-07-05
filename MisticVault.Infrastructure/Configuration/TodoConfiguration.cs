using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MisticVault.Core.Todo.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MisticVault.Infrastructure.Configuration
{
    public class TodoConfiguration : IEntityTypeConfiguration<MisticVault.Core.Todo.Entities.Todo>
    {
        public void Configure(EntityTypeBuilder<MisticVault.Core.Todo.Entities.Todo> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
            builder.Property(t => t.Description).IsRequired();
            builder.Property(t => t.CreatedDate).IsRequired();
            builder.Property(t => t.UpdatedDate).IsRequired(false);
            builder.Property(t => t.CompletedDate).IsRequired(false);
            builder.HasOne(t=>t.Category).WithMany(c=>c.Todos).HasForeignKey(t=>t.CategoryId).OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
