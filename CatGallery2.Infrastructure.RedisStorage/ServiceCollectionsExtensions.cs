using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CatGallery2.Infrastructure.RedisStorage;

public static class ServiceCollectionsExtensions
{
    public static void AddRedisStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredSection(RedisRepositoryOptions.SectionName).Get<RedisRepositoryOptions>();
        
        services.AddSingleton<IViewsRepository, CachingRepository>();
        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(options.ConnectionString));
        services.AddHostedService<CachingStartupService>();
    }
}