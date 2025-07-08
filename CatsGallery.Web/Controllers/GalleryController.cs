using CatsGallery.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatsGallery.Web.Controllers;

public class GalleryController : Controller
{
    private readonly IGalleryState _galleryState;
    private const int InitialCatCount = 3;
    public GalleryController(IGalleryState galleryState)
    {
        _galleryState = galleryState;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Init()
    {
        var cats = await _galleryState.InitializeAsync(InitialCatCount);
        
        return Json(new { 
            prevCat = Convert.ToBase64String(cats[0].ImageBytes),
            currCat = Convert.ToBase64String(cats[1].ImageBytes),
            nextCat = Convert.ToBase64String(cats[2].ImageBytes)
        });
    }
    
    [HttpGet]
    public async Task<IActionResult> Next()
    {
        var cat = await _galleryState.AddNewCatAsync();
        
        return Json(new { 
            nextCat = Convert.ToBase64String(cat.ImageBytes)
        });
    }
    
    
}