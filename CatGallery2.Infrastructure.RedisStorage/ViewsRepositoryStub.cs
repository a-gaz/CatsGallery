using CatGallery2.Application.Services.Interfaces;

namespace CatGallery2.Infrastructure.RedisStorage;

internal sealed class ViewsRepositoryStub : IViewsRepository
{
    private readonly List<UserViewedCat> _userViewedCats = new List<UserViewedCat>();
    
    public Task AddAsync(UserViewedCat view, CancellationToken cancellationToken)
    {
        _userViewedCats.Add(view);
        
        return Task.CompletedTask;
    }

    public Task<long[]> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userViewedCats
            .Where(x => x.UserId == userId)
            .Select(x => x.CatId)
            .ToArray());
    }
}