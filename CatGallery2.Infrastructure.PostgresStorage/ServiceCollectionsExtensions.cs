using CatGallery2.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatGallery2.Infrastructure.PostgresStorage;

public static class ServiceCollectionsExtensions
{
    public static void AddPostgresStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredSection(PostgreRepositoryOptions.SectionName).Get<PostgreRepositoryOptions>();
        
        services.AddDbContext<ApplicationDbContext>(opts =>
        {
            opts.UseNpgsql(options.ConnectionString);
        });
        
        services.AddScoped<ICatImageRepository, CatImageRepository>();
    }
}