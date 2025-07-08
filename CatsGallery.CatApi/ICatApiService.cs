using CatsGallery.Application.Models;

namespace CatsGallery.Gateway;

public interface ICatApiService
{
    Task<CatResponse> GetRandomCatAsync();
    Task<byte[]> GetCatImageByIdAsync(string id);
}