using CatGallery2.Application.Interfaces.Services;
using CatGallery2.Web.ViewModelBuilders.GalleryStrategies;
using CatGallery2.Web.ViewModelBuilders.ModelBuildersAbstract;
using CatGallery2.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CatGallery2.Web.Controllers;

public class GalleryController : Controller
{
    private readonly IGalleryService _galleryService;
    private readonly IViewModelBuilder<GalleryViewModel, IGalleryStrategy> _viewModelBuilder;
    private readonly ILogger<GalleryController> _logger;
    
    public GalleryController(IGalleryService  galleryService,
        IViewModelBuilder<GalleryViewModel, IGalleryStrategy> viewModelBuilder,
        ILogger<GalleryController> logger)
    {
        _galleryService = galleryService;
        _viewModelBuilder = viewModelBuilder;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            await _galleryService.InitializeAsync(cancellationToken);
            
            var userName = User.Identity.Name;
            var model = await _viewModelBuilder
                .WithStrategy(new InitCatsStrategy())
                .BuildAsync(userName, cancellationToken);
            
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
            var userName = User.Identity.Name;
            var model = await _viewModelBuilder
                .WithStrategy(new PrevCatsStrategy())
                .BuildAsync(userName, cancellationToken);
            
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
            var userName = User.Identity.Name;
            var model = await _viewModelBuilder
                .WithStrategy(new NextCatsStrategy())
                .BuildAsync(userName, cancellationToken);

            return PartialView("GalleryPartial", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message); 
            return View("Error", ex);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFile(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var catImageBytes = await _galleryService.GetCatImageBytesAsync(fileName, cancellationToken);
            
            return File(catImageBytes, "image/jpeg");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return View("Error", ex);
        }
    }
}