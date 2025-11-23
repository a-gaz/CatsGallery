using CatGallery2.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatGallery2.Infrastructure.PostgresRepository.Configuration;

public class TagProductConfiguration : IEntityTypeConfiguration<TagProduct>
{
    public void Configure(EntityTypeBuilder<TagProduct> builder)
    {
        builder.HasKey(x => new { x.TagId, x.CatProductId });
        
        builder.HasOne(x => x.Tag)
            .WithMany(t => t.TagProducts)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.SetNull) ;
        
        builder.HasOne(x => x.CatProduct)
            .WithMany(c => c.TagProducts)
            .HasForeignKey(x => x.CatProductId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Property(cp => cp.TagId)
            .IsRequired();
        builder.Property(cp => cp.CatProductId)
            .IsRequired();
    }
}