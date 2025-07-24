using CatGallery2.Application.Services.Interfaces;
using CatGallery2.Application.Services.Realizations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatGallery2.Application;

public static class ServiceCollectionsExtensions 
{
    public static void AddApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICatImageUploadQueue, CatImageUploadQueue>();
        services.AddScoped<ICatService, CatService>();
        services.AddHostedService<CatUploadBackgroundService>();
    }
}