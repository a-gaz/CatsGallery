using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatGallery2.Infrastructure.RedisStorage;

public static class ServiceCollectionsExtensions
{
    public static void AddRedisStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IViewsRepository, ViewsRepositoryStub>();
    }
}