using System.Net.Http.Json;
using CatGallery2.Application.Interfaces.Gateways;

namespace CatGallery2.Infrastructure.CatApi;

internal sealed class CatApiService : ICatProvider
{
    private readonly HttpClient _httpClient;
    
    public CatApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<CatResponse>> GetRandomCatsOneByOneAsync(int pageSize, CancellationToken cancellationToken)
    {
        var jsonResponses = new List<CatResponse>();
        
        for (var i = 0; i < pageSize; i++)
        {
            var response = await _httpClient.GetAsync($"/cat?json=true", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("НЕТ КОТА!");
            }
            var jsonResponse = await response.Content.ReadFromJsonAsync<CatResponse>(cancellationToken);
            
            jsonResponses.Add(jsonResponse);
        }
        
        return jsonResponses;
    }

    public async Task<Stream> GetImageByIdAsync(string id, CancellationToken cancellationToken)
    {
        var imageResponse = await _httpClient.GetAsync($"/cat/{id}", cancellationToken);
        return await imageResponse.Content.ReadAsStreamAsync(cancellationToken);
    }
}