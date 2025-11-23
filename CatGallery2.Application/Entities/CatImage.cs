namespace CatGallery2.Application.Entities;

public sealed class CatImage
{
    public long Id { get; set; }
    public string? ForeignId { get; set; }
    public string? FileName { get; set; }
    public long CatProductId { get; set; }
    public CatProduct CatProduct { get; set; }
}