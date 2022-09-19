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
    internal class ProductFeatureConfiguration : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i=>i.Id).UseIdentityColumn();
            builder.HasOne(i => i.Product).WithOne(i => i.ProductFeature).HasForeignKey<ProductFeature>(i => i.ProductId);
        }
    }
}
