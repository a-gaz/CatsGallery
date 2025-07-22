using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatGallery2.Infrastructure.PostgresStorage;

public static class ServiceCollectionsExtensions
{
    public static void AddPostgresStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICatRepository, CatRepositoryStub>();
    }
}