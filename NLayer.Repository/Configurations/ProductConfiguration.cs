using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).UseIdentityColumn();
            builder.Property(i => i.Name).IsRequired().HasMaxLength(200);
            builder.Property(i => i.Stock).IsRequired();
            builder.Property(i => i.Price).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasOne(i => i.Category).WithMany(i => i.Products).HasForeignKey(i => i.CategoryId);
        }
    }
}
