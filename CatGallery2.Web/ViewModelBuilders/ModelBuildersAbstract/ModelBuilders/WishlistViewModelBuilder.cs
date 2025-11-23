using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Services;
using CatGallery2.Web.ViewModelBuilders.GalleryStrategies;
using CatGallery2.Web.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CatGallery2.Web.ViewModelBuilders.ModelBuildersAbstract.ModelBuilders;

public sealed class WishlistViewModelBuilder : ViewModelBuilder<WishlistViewModel, IWishlistStrategy>
{
    public WishlistViewModelBuilder(IGalleryService galleryService,
        IWishlistService wishlistService,
        UserManager<ApplicationUser> userManager,
        ILogger<ViewModelBuilder<WishlistViewModel, IWishlistStrategy>> logger)
        : base(galleryService, wishlistService, userManager, logger)
    {
    }

    public override async Task<WishlistViewModel> BuildAsync(string? userName, CancellationToken cancellationToken)
    {
        await Validate(userName);
        
        var catProducts = await GetCatProductsAsync(cancellationToken);
        var viewModels = await CreateViewModelsAsync(catProducts, cancellationToken);
        
        return new WishlistViewModel(viewModels);
    }

    private async Task<CatProduct[]> GetCatProductsAsync(CancellationToken cancellationToken)
    {
        if (_strategy == null)
            throw new InvalidOperationException("Стратегия не инициализирована");
    
        var catProduct = await _strategy.GetWishlistItemsAsync(
            _wishlistService,
            _currentUser!.Id,
            cancellationToken);
        return catProduct; 
    }
    
    private async Task<CatProductViewModel[]> CreateViewModelsAsync(CatProduct[] catProducts, CancellationToken cancellationToken)
    {
        if (_currentUser == null)
        {
            throw new Exception("Нет пользователя при доступе к wishlist!");
        }
        
        var catsInWishlist = await GalleryService.CheckWishlistAsync(_currentUser.Id, catProducts, cancellationToken);

        return catProducts
            .Zip(catsInWishlist, (catProduct, inWishlist) => new CatProductViewModel(catProduct, inWishlist))
            .ToArray();
    }
}