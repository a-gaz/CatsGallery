namespace CatGallery2.Infrastructure.PostgresRepository;

internal sealed class CatImageRepositoryOptions
{
    public string ConnectionString { get; set; }
    public static readonly string SectionName = nameof(CatImageRepositoryOptions);
}