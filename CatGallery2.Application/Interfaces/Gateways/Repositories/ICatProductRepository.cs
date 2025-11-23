using CatGallery2.Application.Entities;

namespace CatGallery2.Application.Interfaces.Gateways.Repositories;

public interface ICatProductRepository
{
    Task<long> CreateAsync(CatProduct catProduct, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(long catProductId, CancellationToken cancellationToken);
    Task<bool> CatProductInDb(long catProductId, CancellationToken cancellationToken);
    Task<CatProduct[]> GetCatProductsByIdAsync(long[] catPriductIds, CancellationToken cancellationToken);
    Task<CatProduct[]> GetCatsAsync(int catsNum, DateTime from, long[] viewedCatProducts, CancellationToken cancellationToken);
}