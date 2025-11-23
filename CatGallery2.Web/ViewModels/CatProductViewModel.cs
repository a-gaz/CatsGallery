using CatGallery2.Application.Entities;

namespace CatGallery2.Web.ViewModels;

public sealed class CatProductViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<CatImage> CatImages { get; set; } = new List<CatImage>();
    public ICollection<TagProduct>? TagProducts { get; set; } = new List<TagProduct>();
    // public ICollection<WishlistCat>? WishlistCats { get; set; } = new List<WishlistCat>();
    public bool IsInWishlist { get; set; }
    
    public CatProductViewModel(CatProduct catProduct, bool isInWishlist)
    {
        Name =  catProduct.Name;
        Description = catProduct.Description;
        Price = catProduct.Price;
        CatImages = catProduct.CatImages;
        TagProducts =  catProduct.TagProducts;
        // WishlistCats = new List<WishlistCat>()
        
        IsInWishlist = isInWishlist;
    }
    
    public CatProductViewModel(CatProduct catProduct)
    {
        Name =  catProduct.Name;
        Description = catProduct.Description;
        Price = catProduct.Price;
        CatImages = catProduct.CatImages;
        TagProducts =  catProduct.TagProducts;
        
        IsInWishlist = false;
    }
}