namespace CatGallery2.Infrastructure.CatApi;

internal sealed class CatApiOptions
{
    public string BaseUrl { get; set; }
    public static readonly string SectionName = nameof(CatApiOptions);
}