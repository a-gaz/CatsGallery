
namespace CatGallery2.Web.ViewModels;

public sealed class WishlistViewModel(CatProductViewModel[] сatProductWishlistViewModel)
{
    public CatProductViewModel[] CatProducts { get; set; } = сatProductWishlistViewModel;
}