namespace CatGallery2.Infrastructure.RedisStorage;

internal sealed class RedisRepositoryOptions
{
    public string ConnectionString { get; set; }
    public int DefaultExpiration { get; set; }
    public static readonly string SectionName = nameof(RedisRepositoryOptions);
}