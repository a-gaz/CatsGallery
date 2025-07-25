using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace CatGallery2.Infrastructure.Minio;

public static class ServiceCollectionsExtensions 
{
    public static void AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredSection(MinioRepositoryOptions.SectionName)
            .Get<MinioRepositoryOptions>();
        
        services.AddMinio(opts =>
        {
            opts.WithEndpoint(options.Endpoint);
            opts.WithCredentials(options.AccessKey, options.SecretKey);
            opts.WithSSL(options.UseSsl);
            opts.Build();
        });
        services.AddScoped<IImageStorage, MinioCacheImageStorage>();
    }
}