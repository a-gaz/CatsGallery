using CatGallery2.Application.Services.Entities;
using CatGallery2.Application.Services.Interfaces;
using CatGallery2.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CatGallery2.Web.Controllers;

public class GalleryController : Controller
{
    private readonly ICatService _catService;
    
    public GalleryController(ICatService  catService)
    {
        _catService = catService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var catImages = await _catService.GetNextCatsAsync(3, DateTime.MinValue, Guid.Empty, cancellationToken);
        
        WaitForAll(catImages);

        var model = new GalleryViewModel
        {
            PrevCat = catImages[0],
            CurrCat = catImages[1],
            NextCat = catImages[2],
        };
        
        return View("Index", model);
    }

    public async Task<IActionResult> PrevCat()
    {
        var catImages = await _catService.GetPrevCatsAsync(1, Guid.Empty, CancellationToken.None);
        
        var model = new GalleryViewModel
        {
            PrevCat = catImages[0],
            CurrCat = catImages[1],
            NextCat = catImages[2],
        };
        
        return PartialView("GalleryPartial", model);
    }
    
    public async Task<IActionResult> NextCat()
    {
        var catImages = await _catService.GetNextCatsAsync(1, DateTime.MinValue, Guid.Empty, CancellationToken.None);
        
        WaitForAll(catImages);
       
        var model = new GalleryViewModel
        {
            PrevCat = catImages[0],
            CurrCat = catImages[1],
            NextCat = catImages[2],
        };

        return PartialView("GalleryPartial", model);
    }

    private void WaitForAll(CatImage[] catImages)
    {
        if (catImages.Length < 3)
        {
            throw new Exception("No more cats!");
        }
        
        foreach (var catImage in catImages)
        {
            while (catImage.FileName == null)
            {
                Thread.Sleep(20);
            }
        }
    }
}