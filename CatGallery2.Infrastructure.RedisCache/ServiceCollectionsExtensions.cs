using CatGallery2.Application.Interfaces.Gateways;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CatGallery2.Infrastructure.RedisCache;

public static class ServiceCollectionsExtensions
{
    public static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredSection(UserActivityCacheOptions.SectionName).Get<UserActivityCacheOptions>();
        if (options == null)
        {
            throw new NullReferenceException($"{nameof(UserActivityCacheOptions)}.{nameof(UserActivityCacheOptions.SectionName)}");
        }
        
        services.Configure<UserActivityCacheOptions>(configuration.GetSection(UserActivityCacheOptions.SectionName));
        
        services.AddSingleton<IUserActivityRepository, UserActivityCache>();
        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(options.ConnectionString));
    }
}