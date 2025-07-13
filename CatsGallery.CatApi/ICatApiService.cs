using CatsGallery.Gateway.Models;

namespace CatsGallery.Gateway;

public interface ICatApiService
{
    Task<CatResponse> GetRandomCatAsync();
    Task<byte[]> GetCatImageByIdAsync(string id);
}