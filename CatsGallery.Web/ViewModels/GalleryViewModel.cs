using CatsGallery.Application.Models;

namespace CatsGallery.Web.ViewModels;

public class GalleryViewModel
{
    public Cat PrevCat { get; set; }
    public Cat CurrCat { get; set; }
    public Cat NextCat { get; set; }
}