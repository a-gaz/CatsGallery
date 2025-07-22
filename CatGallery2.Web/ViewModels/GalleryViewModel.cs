using CatGallery2.Application.Services.Entities;

namespace CatGallery2.Web.ViewModels;

public class GalleryViewModel
{
    public CatImage PrevCat { get; set; }
    public CatImage CurrCat { get; set; }
    public CatImage NextCat { get; set; }
}