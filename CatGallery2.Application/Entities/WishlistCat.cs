namespace CatGallery2.Application.Entities;

public sealed class WishlistCat
{
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public long CatProductId { get; set; }
    public CatProduct CatProduct { get; set; }
}