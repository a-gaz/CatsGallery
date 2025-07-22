using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatGallery2.Infrastructure.Minio;

public static class ServiceCollectionsExtensions 
{
    public static void AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IImageStorage, MinioImageStorageStub>();
    }
}