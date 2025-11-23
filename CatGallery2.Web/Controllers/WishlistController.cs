using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Services;
using CatGallery2.Web.ViewModelBuilders.GalleryStrategies;
using CatGallery2.Web.ViewModelBuilders.ModelBuildersAbstract;
using CatGallery2.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CatGallery2.Web.Controllers;

public class WishlistController : Controller
{
    private readonly IWishlistService _wishlistService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IViewModelBuilder<WishlistViewModel, IWishlistStrategy> _viewModelBuilder;
    private readonly ILogger<GalleryController> _logger;
    public WishlistController(UserManager<ApplicationUser> userManager, 
        IWishlistService wishlistService,
        IViewModelBuilder<WishlistViewModel, IWishlistStrategy> viewModelBuilder,
        ILogger<GalleryController> logger) 
    {
        _viewModelBuilder = viewModelBuilder;
        _logger = logger;
        _userManager = userManager;
        _wishlistService = wishlistService;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetWishlist(CancellationToken cancellationToken)
    {
        var userName = User.Identity.Name;
        var model = await _viewModelBuilder
            .WithStrategy(new LimitedWishlistStrategy())
            .BuildAsync(userName, cancellationToken);
        
        return View("Wishlist", model);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToWishlist(string foreignId, CancellationToken cancellationToken)
    {
        if (User.Identity.IsAuthenticated == false)
        {
            var returnUrl = Url.Action("Login", "Auth");
            return RedirectToAction("Login", "Auth", new { returnUrl });
        }
        else
        {
            var userName = User.Identity.Name;
            var currentUser = await _userManager.FindByNameAsync(userName);

            try
            {
                var res = await _wishlistService.AddAsync(currentUser.Id, foreignId, cancellationToken);
        
                if (res)
                {
                    return Json(new { success = true, message = "Добавлено в избранное" });
                }
                else
                {
                    return await DeleteFromWishlist(foreignId, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteFromWishlist(string foreignId, CancellationToken cancellationToken)
    {
        var userName = User.Identity.Name;
        var currentUser = await _userManager.FindByNameAsync(userName);

        if (currentUser == null)
        {
            return RedirectToPage("UserName");
        }
        else
        {
            try
            {
                var res = await _wishlistService.DeleteAsync(currentUser.Id, foreignId, cancellationToken);
                if (res)
                {
                    return Json(new { success = true, message = "Удалено из избранного" });
                }
                else
                {
                    return Json(new { success = false, message = "Уже удален или другая ошибка" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Ошибка: {ex.Message}" });
            }
        }
    }
}