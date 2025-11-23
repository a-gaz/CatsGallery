using CatGallery2.Application.Interfaces.Gateways.Repositories;
using CatGallery2.Infrastructure.PostgresRepository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatGallery2.Infrastructure.PostgresRepository;

public static class ServiceCollectionsExtensions
{
    public static void AddPostgresRepository(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredSection(CatImageRepositoryOptions.SectionName)
            .Get<CatImageRepositoryOptions>() ?? throw new NullReferenceException();
    
        services.AddDbContextFactory<ApplicationDbContext>(opts => 
        {
            opts.UseNpgsql(options.ConnectionString);
        });
    
        services.AddSingleton<ApplicationDbContextFactory>();
        services.AddSingleton<ICatImageRepository, CatImageRepository>();
        services.AddSingleton<ICatProductRepository, CatProductRepository>();
        services.AddSingleton<IWishlistRepository, WishlistRepository>();
    }
}