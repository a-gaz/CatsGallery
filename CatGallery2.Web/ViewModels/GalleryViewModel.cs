namespace CatGallery2.Web.ViewModels;

public sealed class GalleryViewModel
{
    public CatProductViewModel PrevCat { get; set; }
    public CatProductViewModel CurrCat { get; set; }
    public CatProductViewModel NextCat { get; set; }
}