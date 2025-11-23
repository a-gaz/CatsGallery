using CatGallery2.Application.Interfaces.Gateways;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace CatGallery2.Infrastructure.MinioStorage;

public static class ServiceCollectionsExtensions 
{
    public static void AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredSection(MinioRepositoryOptions.SectionName).Get<MinioRepositoryOptions>();
        if (options == null)
        {
            throw new NullReferenceException($"{nameof(MinioRepositoryOptions)}.{nameof(MinioRepositoryOptions.SectionName)}");
        }
        
        services.Configure<MinioRepositoryOptions>(configuration.GetSection(MinioRepositoryOptions.SectionName));
        
        services.AddMinio(opts =>
        {
            opts.WithEndpoint(options.Endpoint);
            opts.WithCredentials(options.AccessKey, options.SecretKey);
            opts.WithSSL(options.UseSsl);
            opts.Build();
        });
        services.AddSingleton<IImageStorage, MinioCacheImageStorage>();
    }
}