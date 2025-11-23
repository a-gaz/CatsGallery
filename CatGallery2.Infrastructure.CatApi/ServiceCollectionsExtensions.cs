using CatGallery2.Application.Interfaces.Gateways;
using CatGallery2.Application.Interfaces.Services;
using CatGallery2.Application.Realizations.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatGallery2.Infrastructure.CatApi;

public static class ServiceCollectionsExtensions
{
    public static void AddCatApi(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredSection(CatApiOptions.SectionName).Get<CatApiOptions>();
        if (options == null)
        {
            throw new NullReferenceException($"{nameof(CatApiOptions)}.{nameof(CatApiOptions.SectionName)}");
        }
        
        services.AddSingleton<ICatProvider, CatApiService>();
        services.AddHttpClient<ICatProvider, CatApiService>(x => x.BaseAddress = new Uri(options.BaseUrl));
        
        services.AddSingleton<IWishlistService, WishlistService>();
    }
}