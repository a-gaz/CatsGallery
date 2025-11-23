using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Services;

namespace CatGallery2.Web.ViewModelBuilders.GalleryStrategies;

public interface IWishlistStrategy
{
    Task<CatProduct[]> GetWishlistItemsAsync(IWishlistService wishlistService, string userId, CancellationToken cancellationToken);
}
// todo потом надо будет загружать по несколько котов (page)
public class LimitedWishlistStrategy : IWishlistStrategy
{
    public async Task<CatProduct[]> GetWishlistItemsAsync(IWishlistService wishlistService, string userId, CancellationToken cancellationToken)
    {
        return await wishlistService.GetAsync(userId, cancellationToken);
    }
}
