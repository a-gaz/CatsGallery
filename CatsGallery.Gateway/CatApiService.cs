namespace CatsGallery.Gateway;

public class CatApiService : ICatApiService
{
    private readonly HttpClient _httpClient;

    public CatApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://cataas.com/");
    }

    public async Task<HttpResponseMessage> GetRandomCatAsync()
    {
        return await _httpClient.GetAsync("/cat?json=true");
    }

    public async Task<HttpResponseMessage> GetCatByIdAsync(string id)
    {
        return await _httpClient.GetAsync($"/cat/{id}");
    }
}
