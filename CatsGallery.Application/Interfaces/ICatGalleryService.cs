using CatsGallery.Application.Entities;

namespace CatsGallery.Application.Interfaces;

public interface ICatGalleryService
{
    void GetOrAddUserStorage(string userId);
    Task<Cat[]> GetInitialCatsAsync(string userId);
    Task<Cat> GetNextCatAsync(string userId);
}