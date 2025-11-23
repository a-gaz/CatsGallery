using Microsoft.EntityFrameworkCore;

namespace CatGallery2.Infrastructure.PostgresRepository;

internal class ApplicationDbContextFactory
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public ApplicationDbContextFactory(DbContextOptions<ApplicationDbContext> options)
    {
        _options = options;
    }

    public ApplicationDbContext CreateDbContext()
    {
        var context = new ApplicationDbContext(_options);
        context.Database.EnsureCreated();
        return context;
    }
}