using CatGallery2.Application.Entities;

namespace CatGallery2.Application.Interfaces.Services;

public interface IGalleryService
{
    Task InitializeAsync(CancellationToken cancellationToken);
    Task<CatProduct[]> GetNextCatsAsync(int catsNum, DateTime from, Guid userId, CancellationToken cancellationToken);
    Task<CatProduct[]> GetPrevCatsAsync(int catsNum, Guid userId, CancellationToken cancellationToken);
    Task<byte[]> GetCatImageBytesAsync(string fileName, CancellationToken cancellationToken);
    Task<bool[]> CheckWishlistAsync(string currentUserId, CatProduct[] catImages, CancellationToken cancellationToken);
}