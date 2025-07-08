using System.Text.Json;
using CatsGallery.Application.Models;
using CatsGallery.Gateway;

namespace CatsGallery.Application;

public class CatService : ICatService
{
    private readonly ICatApiService _catApiService;

    public CatService(ICatApiService catApiService)
    {
        _catApiService = catApiService;
    }
    
    public async Task<Cat> GetRandomCatAsync()
    {
        var catData = await _catApiService.GetRandomCatAsync();
        var imageBytes = await _catApiService.GetCatImageByIdAsync(catData.Id);
        
        return new Cat
        {
            Id = catData.Id,
            Tags = catData.Tags.ToList(),
            ImageBytes = imageBytes
        };
    }
} 