using CatGallery2.Application.Services.Entities;
using CatGallery2.Application.Services.Interfaces;
using CatGallery2.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CatGallery2.Web.Controllers;

public class GalleryController : Controller
{
    private readonly ICatService _catService;
    private readonly ILogger<GalleryController> _logger;

    public GalleryController(ICatService  catService, ILogger<GalleryController> logger)
    {
        _catService = catService;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            await _catService.InitializeAsync(cancellationToken);
            
            var catImages = await _catService.GetNextCatsAsync(3, DateTime.MinValue, Guid.Empty, cancellationToken);

            var model = BuildModel(catImages);
        
            return View("Index", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return View("Error", ex);
        }
    }

    public async Task<IActionResult> PrevCat(CancellationToken cancellationToken)
    {
        try
        {
            var catImages = await _catService.GetPrevCatsAsync(1, Guid.Empty, cancellationToken);
            
            var model = BuildModel(catImages);
            
            return PartialView("GalleryPartial", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return View("Error", ex);
        }
    }
    
    public async Task<IActionResult> NextCat(CancellationToken cancellationToken)
    {
        try
        {
            var catImages = await _catService.GetNextCatsAsync(1, DateTime.MinValue, Guid.Empty, cancellationToken);

            var model = BuildModel(catImages);

            return PartialView("GalleryPartial", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message); 
            return View("Error", ex);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetLink(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var memoryStream = await _catService.GetUrlAsync(fileName, cancellationToken);
            
            return File(memoryStream, "image/jpeg", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return View("Error", ex);
        }
    }
    
    private static GalleryViewModel BuildModel(CatImage[] catImages)
    {
        return new GalleryViewModel
        {
            PrevCat = catImages[0],
            CurrCat = catImages[1],
            NextCat = catImages[2],
        };
    }
}