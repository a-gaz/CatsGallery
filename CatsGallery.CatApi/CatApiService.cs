using System.Text.Json;
using CatsGallery.Gateway.Models;

namespace CatsGallery.Gateway;

public class CatApiService : ICatApiService
{
    private readonly HttpClient _httpClient;

    public CatApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://cataas.com/");
    }

    public async Task<CatResponse> GetRandomCatAsync()
    {
        var response = await _httpClient.GetAsync("/cat?json=true");

        if (!response.IsSuccessStatusCode) 
            return null;
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var catData = JsonSerializer.Deserialize<CatResponse>(jsonResponse);

        return catData;
    }

    public async Task<byte[]> GetCatImageByIdAsync(string id)
    {
        var imageResponse = await _httpClient.GetAsync($"/cat/{id}");
        var imageBytes = await imageResponse.Content.ReadAsByteArrayAsync();
        return imageBytes;
    }
}
