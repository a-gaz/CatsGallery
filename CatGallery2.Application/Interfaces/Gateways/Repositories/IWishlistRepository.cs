using CatGallery2.Application.Entities;

namespace CatGallery2.Application.Interfaces.Gateways.Repositories;

public interface IWishlistRepository
{
    Task<CatProduct[]> GetAsync(string userId, CancellationToken cancellationToken);
    Task<bool> CheckCatInDbAsync(string userId, long catProductId, CancellationToken cancellationToken);
    Task<bool> AddAsync(string userId, long catProductId, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string userId, long catProductId, CancellationToken cancellationToken);
}