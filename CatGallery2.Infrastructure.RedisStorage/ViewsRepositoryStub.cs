using CatGallery2.Application.Services.Entities;
using CatGallery2.Application.Services.Interfaces;

namespace CatGallery2.Infrastructure.RedisStorage;

internal sealed class ViewsRepositoryStub : IViewsRepository
{
    private readonly List<UserViewedCat> _userViewedCats = new List<UserViewedCat>();
    
    public Task AddAsync(Guid userId, CatImage[] catImages, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<long[]> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userViewedCats
            .Where(x => x.UserId == userId)
            .Select(x => x.CatId)
            .ToArray());
    }

    Task<long> IViewsRepository.GetUserCurrCatViewIndexAsync(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetUserCurrCatViewIndexAsync(Guid userId, long catImageId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}