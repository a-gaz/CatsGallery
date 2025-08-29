using CatGallery2.Application.Services.Interfaces;
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
    }
}