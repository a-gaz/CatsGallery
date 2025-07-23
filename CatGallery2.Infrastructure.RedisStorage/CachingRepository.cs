using CatGallery2.Application.Services.Entities;
using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CatGallery2.Infrastructure.RedisStorage;

internal sealed class CachingRepository : IViewsRepository
{
    private readonly IDatabase _cache;
    private readonly TimeSpan _defaultExpiration;

    private const string KeyViewsDb = "userviews:";
    private const string KeyPointersDb = "userpointers:";

    public CachingRepository(IConnectionMultiplexer distributedCache, IConfiguration configuration)
    {
        _cache = distributedCache.GetDatabase();
        
        var options = configuration.GetRequiredSection(RedisRepositoryOptions.SectionName).Get<RedisRepositoryOptions>();
        _defaultExpiration = TimeSpan.FromMinutes(options.DefaultExpiration);
    }
    
    public async Task AddAsync(Guid userId, CatImage[] catImages, CancellationToken cancellationToken)
    {
        var key = $"{KeyViewsDb}{userId.ToString()}";
        
        foreach (var catImage in catImages)
        {
            await _cache.ListRightPushAsync(key, catImage.Id.ToString());
        }
        
        await _cache.KeyExpireAsync(key, _defaultExpiration);
    }

    public async Task<long[]> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var key = $"{KeyViewsDb}{userId.ToString()}";
        
        var viewedCatIds = await _cache.ListRangeAsync(key);

        if (viewedCatIds.Length == 0)
        {
            return [];
        }
        
        return viewedCatIds.Select(rv => long.Parse(rv.ToString())).ToArray();
    }

    public async Task SetUserCurrCatViewIndexAsync(Guid userId, long catImageId, CancellationToken cancellationToken)
    {
        var key = $"{KeyPointersDb}{userId.ToString()}";
        
        await _cache.StringSetAsync(key, catImageId);
        await _cache.KeyExpireAsync(key, _defaultExpiration);
    }

    public async Task<long> GetUserCurrCatViewIndexAsync(Guid userId, CancellationToken cancellationToken)
    {
        var key = $"{KeyPointersDb}{userId.ToString()}";
        
        var indexValue = await _cache.StringGetAsync(key);

        if (indexValue.IsNullOrEmpty)
        {
            return -1;
        }

        return int.Parse(indexValue.ToString());
    }
}