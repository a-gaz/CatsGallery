using CatsGallery.Application.Entities;

namespace CatsGallery.Application.Interfaces;

public interface ICatApiService
{
    IAsyncEnumerable<Cat[]> GetAllCatsPaginatedAsync(int limit);
}