﻿using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatGallery2.Infrastructure.CatApi;

public static class ServiceCollectionsExtensions
{
    public static void AddCatApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICatProvider, CatApiService>();
        
        var options = configuration.GetRequiredSection(CatApiOptions.SectionName).Get<CatApiOptions>();
        services.AddHttpClient<ICatProvider, CatApiService>(x => x.BaseAddress = new Uri(options.BaseUrl));
    }
}