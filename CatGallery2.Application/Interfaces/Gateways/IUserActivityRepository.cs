using CatGallery2.Application.Entities;

namespace CatGallery2.Application.Interfaces.Gateways;

public interface IUserActivityRepository
{
    Task AddAsync(Guid userId, CatProduct[] catProducts, CancellationToken cancellationToken);
    Task<long[]> GetByUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<long> GetUserCurrCatViewIndexAsync(Guid userId, CancellationToken cancellationToken);
    Task SetUserCurrCatViewIndexAsync(Guid userId, long catProductId, CancellationToken cancellationToken);
}

public sealed record UserViewedCat(Guid UserId, long CatId);