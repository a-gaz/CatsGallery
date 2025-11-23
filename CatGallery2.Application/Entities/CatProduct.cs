namespace CatGallery2.Application.Entities;

public sealed class CatProduct
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? UploadDate { get; set; }
    public decimal Price { get; set; }
    
    public List<CatImage> CatImages { get; set; } = new List<CatImage>();
    public ICollection<TagProduct>? TagProducts { get; set; } = new List<TagProduct>();
    public ICollection<WishlistCat>? WishlistCats { get; set; } = new List<WishlistCat>();
    
    public CatProductSource Source { get; set; }
    
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public void UpdateTagProductIds(long newCatProductId)
    {
        foreach (var tagProduct in TagProducts)
        {
            tagProduct.CatProductId = newCatProductId;
        }
    }
}