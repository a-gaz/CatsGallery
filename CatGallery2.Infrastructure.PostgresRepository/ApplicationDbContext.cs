using CatGallery2.Application.Services.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatGallery2.Infrastructure.PostgresRepository;

internal sealed class ApplicationDbContext : DbContext
{
    public DbSet<CatImage> CatImages { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatImage>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });
    }
}