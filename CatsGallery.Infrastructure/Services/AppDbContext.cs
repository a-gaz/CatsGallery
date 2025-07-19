using Microsoft.EntityFrameworkCore;

namespace CatsGallery.Infrastructure.Services;

public class AppDbContext: DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

}