using System.Text.Json;
using CatsGallery.Application.Entities;
using CatsGallery.Application.Interfaces;
using CatsGallery.Infrastructure.Entities;

namespace CatsGallery.Infrastructure.Services;

public class CatApiService : ICatApiService
{
    private readonly HttpClient _httpClient;

    public CatApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        _httpClient.BaseAddress = new Uri("https://cataas.com/");
    }
    
    public async IAsyncEnumerable<Cat[]> GetAllCatsPaginatedAsync(int limit)
    {
        while (true)
        {
            var response = await _httpClient.GetAsync($"/api/cats?json=true&limit={limit}");
            if (!response.IsSuccessStatusCode) yield break;
            
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var catsResponsePage = JsonSerializer.Deserialize<CatResponse[]>(jsonResponse);

            if (catsResponsePage == null || catsResponsePage.Length == 0) yield break;

            var catsPage = new List<Cat>();
            foreach (var catResponse in catsResponsePage)
            {
                var imageBytes = await GetCatImageByIdAsync(catResponse.Id);
                catsPage.Add(new Cat
                {
                    Id = catResponse.Id,
                    Tags = catResponse.Tags.ToList(),
                    ImageBytes = imageBytes
                });
            }
            
            yield return catsPage.ToArray();
        }
    }

    private async Task<byte[]> GetCatImageByIdAsync(string id)
    {
        var imageResponse = await _httpClient.GetAsync($"/cat/{id}");
        var imageBytes = await imageResponse.Content.ReadAsByteArrayAsync();
        return imageBytes;
    }
}