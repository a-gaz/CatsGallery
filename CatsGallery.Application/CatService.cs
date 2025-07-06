using System.Text.Json;
using CatsGallery.Application.Models;
using CatsGallery.Gateway;

namespace CatsGallery.Application;

public class CatService : ICatService
{
    private readonly ICatApiService _catApiService;

    private static int i = 0;
    public CatService(ICatApiService catApiService)
    {
        _catApiService = catApiService;
    }
    
    public async Task<Cat> GetRandomCatAsync()
    {
        var response = await _catApiService.GetRandomCatAsync();

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var catData = JsonSerializer.Deserialize<CatResponse>(jsonResponse);

            var imageResponse = await _catApiService.GetCatByIdAsync(catData.id);
            var imageBytes = await imageResponse.Content.ReadAsByteArrayAsync();

            i++;
        
            return new Cat
            {
                Id = i,
                Tags = catData.tags,
                ImageBytes = imageBytes
            };
        }

        throw new ApplicationException("Не был получен кот!");
    }
}