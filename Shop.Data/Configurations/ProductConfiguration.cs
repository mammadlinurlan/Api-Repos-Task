using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(c => c.Name).HasMaxLength(50).IsRequired();
            builder.Property(c => c.IsDeleted).HasDefaultValue(false);
            builder.Property(c => c.Image).HasMaxLength(100).IsRequired();
            builder.HasOne(x => x.Category).WithMany(x => x.Products).OnDelete(DeleteBehavior.Cascade);
            builder.Property(c => c.CostPrice).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(c => c.SalePrice).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(c => c.ModifiedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
