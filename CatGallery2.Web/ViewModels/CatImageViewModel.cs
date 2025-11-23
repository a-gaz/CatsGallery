using CatGallery2.Application.Entities;

namespace CatGallery2.Web.ViewModels;

public class CatImageViewModel
{
    public CatImage CatImage { get; set; }
    public bool IsInWishlist { get; set; }

    public CatImageViewModel(CatImage catImage, bool isInWishlist)
    {
        CatImage = catImage;
        IsInWishlist = isInWishlist;
    }
    
    public CatImageViewModel(CatImage catImage)
    {
        CatImage = catImage;
        IsInWishlist = false;
    }
}