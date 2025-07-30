namespace CatGallery2.Infrastructure.RedisCache;

internal sealed class UserActivityCacheOptions
{
    public string ConnectionString { get; set; }
    public int DefaultExpiration { get; set; }
    public static readonly string SectionName = nameof(UserActivityCacheOptions);
}