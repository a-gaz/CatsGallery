using CatGallery2.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatGallery2.Infrastructure.PostgresRepository.Configuration;

public class CatImageConfiguration : IEntityTypeConfiguration<CatImage>
{
    public void Configure(EntityTypeBuilder<CatImage> builder)
    {
        // builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.Property(cp => cp.CatProductId)
            .IsRequired();
    }
}