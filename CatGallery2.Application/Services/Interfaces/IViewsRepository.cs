using CatGallery2.Application.Services.Entities;

namespace CatGallery2.Application.Services.Interfaces;

public interface IViewsRepository
{
    Task AddAsync(Guid userId, CatImage[] catImages, CancellationToken cancellationToken);
    Task<long[]> GetByUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<long> GetUserCurrCatViewIndexAsync(Guid userId, CancellationToken cancellationToken);
    Task SetUserCurrCatViewIndexAsync(Guid userId, long catImageId, CancellationToken cancellationToken);
}

public sealed record UserViewedCat(Guid UserId, long CatId);