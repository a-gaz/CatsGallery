using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Services;
using CatGallery2.Web.ViewModelBuilders.GalleryStrategies;
using CatGallery2.Web.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CatGallery2.Web.ViewModelBuilders.ModelBuildersAbstract.ModelBuilders;

public sealed class GalleryViewModelBuilder : ViewModelBuilder<GalleryViewModel, IGalleryStrategy>
{
    public GalleryViewModelBuilder(IGalleryService galleryService, 
        IWishlistService wishlistService, 
        UserManager<ApplicationUser> userManager,
        ILogger<ViewModelBuilder<GalleryViewModel, IGalleryStrategy>> logger) 
        : base(galleryService, wishlistService, userManager, logger)
    {
    }
    
    public override async Task<GalleryViewModel> BuildAsync(string? userName, CancellationToken cancellationToken)
    {
        await Validate(userName);

        var catProducts = await GetCatProductsAsync(cancellationToken);
        var viewModels = await CreateViewModelsAsync(catProducts, cancellationToken);
    
        return new GalleryViewModel
        {
            PrevCat = viewModels[0],
            CurrCat = viewModels[1],
            NextCat = viewModels[2],
        };
    }
    
    private async Task<CatProduct[]> GetCatProductsAsync(CancellationToken cancellationToken)
    {
        var catProducts = await _strategy.GetCatProductsAsync(GalleryService, cancellationToken);
        return catProducts; 
    }

    private async Task<CatProductViewModel[]> CreateViewModelsAsync(CatProduct[] catProducts, CancellationToken cancellationToken)
    {
        if (_currentUser == null)
        {
            return catProducts.Select(catProduct => new CatProductViewModel(catProduct)).ToArray();
        }
    
        var wishlistResults = await GalleryService.CheckWishlistAsync(_currentUser.Id, catProducts, cancellationToken);
    
        return catProducts
            .Zip(wishlistResults, (catProduct, inWishlist) => new CatProductViewModel(catProduct, inWishlist))
            .ToArray();
    }
}