namespace CatGallery2.Application.Services.Interfaces;

public interface IViewsRepository
{
    Task AddAsync(UserViewedCat view, CancellationToken cancellationToken);
    Task<long[]> GetByUserAsync(Guid userId, CancellationToken cancellationToken);
}

public sealed record UserViewedCat(Guid UserId, long CatId);