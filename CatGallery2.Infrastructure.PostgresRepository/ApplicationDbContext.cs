using CatGallery2.Application.Entities;
using CatGallery2.Infrastructure.PostgresRepository.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatGallery2.Infrastructure.PostgresRepository;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
    public DbSet<CatImage> CatImages { get; set; } = null!;
    public DbSet<CatProduct> CatProducts { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<TagProduct> TagProducts { get; set; } = null!;
    public DbSet<WishlistCat> WishlistCats { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new CatImageConfiguration());
        modelBuilder.ApplyConfiguration(new CatProductConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new TagProductConfiguration());
        modelBuilder.ApplyConfiguration(new WishlistCatConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}