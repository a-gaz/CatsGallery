using CatGallery2.Application.Services.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatGallery2.Infrastructure.PostgresStorage;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<CatImage> CatImages { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}