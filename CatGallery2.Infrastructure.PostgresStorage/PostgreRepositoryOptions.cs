namespace CatGallery2.Infrastructure.PostgresStorage;

internal sealed class PostgreRepositoryOptions
{
    public string ConnectionString { get; set; }
    public static readonly string SectionName = nameof(PostgreRepositoryOptions);
}