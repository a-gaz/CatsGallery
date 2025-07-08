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
        var response = await _catApiService.GetRandomCatAsync();

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var catData = JsonSerializer.Deserialize<CatResponse>(jsonResponse); // TODO Еще сама модель CatResponse. Прочитай про рекорды https://metanit.com/sharp/tutorial/3.51.php

            var imageResponse = await _catApiService.GetCatByIdAsync(catData.id);
            var imageBytes = await imageResponse.Content.ReadAsByteArrayAsync();
        
            return new Cat
            {
                Id = catData.id,
                Tags = catData.tags.ToList(),
                ImageBytes = imageBytes
            };
        }

        throw new ApplicationException("Не был получен кот!");
    }
}