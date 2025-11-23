using CatGallery2.Application.Entities;

namespace CatGallery2.Application.Interfaces.Services;

public interface IWishlistService
{
    Task<bool> AddAsync(string userId, string foreignId, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string userId, string fileName, CancellationToken cancellationToken);
    Task<CatProduct[]> GetAsync(string currentUserId, CancellationToken cancellationToken);
}