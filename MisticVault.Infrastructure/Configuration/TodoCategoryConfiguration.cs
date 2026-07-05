using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using MisticVault.Core.Todo.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MisticVault.Infrastructure.Configuration
{
    public class TodoCategoryConfiguration: IEntityTypeConfiguration<MisticVault.Core.Todo.Entities.TodoCategory>
    {
        public void Configure(EntityTypeBuilder<MisticVault.Core.Todo.Entities.TodoCategory> builder)
        {
            builder.HasKey(tc => tc.Id);
            builder.Property(tc => tc.Name).IsRequired().HasMaxLength(100);
            builder.Property(tc => tc.Description).IsRequired().HasMaxLength(500); 
            builder.Property(x => x.Color).HasMaxLength(20);

        }
    }
}
