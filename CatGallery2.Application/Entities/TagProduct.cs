namespace CatGallery2.Application.Entities;

public sealed class TagProduct
{
    public int TagId { get; set; }
    public Tag Tag { get; set; }
    public long CatProductId { get; set; }
    public CatProduct CatProduct { get; set; }
}