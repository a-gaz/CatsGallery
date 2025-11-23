using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Services;

namespace CatGallery2.Web.ViewModelBuilders.GalleryStrategies;

public interface IGalleryStrategy
{
    Task<CatProduct[]> GetCatProductsAsync(IGalleryService galleryService, CancellationToken cancellationToken);
}

public class InitCatsStrategy : IGalleryStrategy
{
    public async Task<CatProduct[]> GetCatProductsAsync(IGalleryService galleryService, CancellationToken cancellationToken)
    {
        return await galleryService.GetNextCatsAsync(3, DateTime.MinValue, Guid.Empty, cancellationToken);
    }
}

public class NextCatsStrategy : IGalleryStrategy
{
    public async Task<CatProduct[]> GetCatProductsAsync(IGalleryService galleryService, CancellationToken cancellationToken)
    {
        return await galleryService.GetNextCatsAsync(1, DateTime.MinValue, Guid.Empty, cancellationToken);
    }
}

public class PrevCatsStrategy : IGalleryStrategy
{
    public async Task<CatProduct[]> GetCatProductsAsync(IGalleryService galleryService, CancellationToken cancellationToken)
    {
        return await galleryService.GetPrevCatsAsync(1, Guid.Empty, cancellationToken);
    }
}
