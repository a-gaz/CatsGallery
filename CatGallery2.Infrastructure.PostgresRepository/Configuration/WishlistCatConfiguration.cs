using CatGallery2.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatGallery2.Infrastructure.PostgresRepository.Configuration;

public class WishlistCatConfiguration : IEntityTypeConfiguration<WishlistCat>
{
    public void Configure(EntityTypeBuilder<WishlistCat> builder)
    {
        builder.HasKey(x => new { x.ApplicationUserId, x.CatProductId });

        builder.HasOne(x => x.ApplicationUser)
            .WithMany(y => y.WishlistCats)
            .HasForeignKey(x => x.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.CatProduct)
            .WithMany(y => y.WishlistCats)
            .HasForeignKey(x => x.CatProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(cp => cp.ApplicationUserId)
            .IsRequired();
        builder.Property(cp => cp.CatProductId)
            .IsRequired();
    }
}