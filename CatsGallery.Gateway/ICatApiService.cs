namespace CatsGallery.Gateway;

public interface ICatApiService
{
    Task<HttpResponseMessage> GetRandomCatAsync();
    Task<HttpResponseMessage> GetCatByIdAsync(string id);
}