using CatGallery2.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatGallery2.Infrastructure.PostgresRepository;

public static class ServiceCollectionsExtensions
{
    public static void AddPostgresRepository(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredSection(CatImageRepositoryOptions.SectionName).Get<CatImageRepositoryOptions>();
        if (options == null)
        {
            throw new NullReferenceException($"{nameof(CatImageRepositoryOptions)}.{nameof(CatImageRepositoryOptions.SectionName)}");
        }
        
        services.AddDbContext<ApplicationDbContext>(opts =>
        {
            opts.UseNpgsql(options.ConnectionString);
        });
        
        services.AddScoped<ICatImageRepository, CatImageRepository>();
    }
}