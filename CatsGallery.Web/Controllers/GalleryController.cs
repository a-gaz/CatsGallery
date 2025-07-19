using CatsGallery.Application.Interfaces;
using CatsGallery.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatsGallery.Web.Controllers;

public class GalleryController : Controller
{
    private readonly IBrowserSession _browserSession;
    private readonly ICatGalleryService _catGalleryService;
    private readonly ILogger<GalleryController> _logger;
    
    public GalleryController(IBrowserSession browserSession, 
        ICatGalleryService catGalleryService,
        ILogger<GalleryController> logger)
    {
        _browserSession = browserSession;
        _catGalleryService = catGalleryService;
        _logger = logger;
    }
    
    public IActionResult Index()
    {
        var currentUserId = _browserSession.GetCurrentUserId();
        
        _logger.LogInformation($"currentUserId: {currentUserId}");
        
        ViewData["CurrentUserId"] = currentUserId;
        
        _catGalleryService.GetOrAddUserStorage(currentUserId);

        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Init()
    {
        var currentUserId = _browserSession.GetCurrentUserId();
        var cats = await _catGalleryService.GetInitialCatsAsync(currentUserId);
        
        return Json(new { 
            prevCat = Convert.ToBase64String(cats[0].ImageBytes),
            currCat = Convert.ToBase64String(cats[1].ImageBytes),
            nextCat = Convert.ToBase64String(cats[2].ImageBytes)
        });
    }
    
    [HttpGet]
    public async Task<IActionResult> Next()
    {
        var currentUserId = _browserSession.GetCurrentUserId();
        var cat = await _catGalleryService.GetNextCatAsync(currentUserId);
        
        return Json(new { 
            nextCat = Convert.ToBase64String(cat.ImageBytes)
        });
    }
}