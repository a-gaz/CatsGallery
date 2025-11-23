using CatGallery2.Application.Interfaces.EntityBuilders;
using CatGallery2.Application.Interfaces.Gateways;
using CatGallery2.Application.Interfaces.Services;
using CatGallery2.Application.Realizations;
using CatGallery2.Application.Realizations.EntityBuilders;
using CatGallery2.Application.Realizations.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatGallery2.Application;

public static class ServiceCollectionsExtensions 
{
    public static void AddApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICatImageUploadQueue, CatImageUploadQueue>();
        services.AddScoped<ICatProductBuilder, CatProductBuilder>();
        services.AddScoped<IGalleryService, GalleryService>();
        services.AddHostedService<CatUploadBackgroundService>();
    }
}