using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Gateways;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CatGallery2.Infrastructure.RedisCache;

internal sealed class UserActivityCache : IUserActivityRepository
{
    private readonly IDatabase _cache;
    private readonly TimeSpan _defaultExpiration;

    private const string KeyViewsDb = "userviews:";
    private const string KeyPointersDb = "userpointers:";

    public UserActivityCache(IConnectionMultiplexer distributedCache, IOptions<UserActivityCacheOptions> options)
    {
        _cache = distributedCache.GetDatabase();
        
        _defaultExpiration = TimeSpan.FromMinutes(options.Value.DefaultExpiration);
    }
    
    public async Task AddAsync(Guid userId, CatProduct[] catProducts, CancellationToken cancellationToken)
    {
        var key = $"{KeyViewsDb}{userId.ToString()}";
        
        foreach (var catProduct in catProducts)
        {
            await _cache.ListRightPushAsync(key, catProduct.Id.ToString());
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

    public async Task SetUserCurrCatViewIndexAsync(Guid userId, long catProductId, CancellationToken cancellationToken)
    {
        var key = $"{KeyPointersDb}{userId.ToString()}";
        
        await _cache.StringSetAsync(key, catProductId);
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