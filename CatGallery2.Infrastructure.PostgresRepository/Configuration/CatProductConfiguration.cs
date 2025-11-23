using CatGallery2.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatGallery2.Infrastructure.PostgresRepository.Configuration;

public class CatProductConfiguration : IEntityTypeConfiguration<CatProduct>
{
    public void Configure(EntityTypeBuilder<CatProduct> builder)
    {
        builder.HasMany(x => x.CatImages)
            .WithOne(x => x.CatProduct)
            .HasForeignKey(x => x.CatProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(cp => cp.ApplicationUserId)
            .IsRequired();
    }
}