using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatGallery2.Infrastructure.PostgresRepository.Repositories;

internal class WishlistRepository : IWishlistRepository
{
    private readonly ApplicationDbContextFactory _contextFactory;
    private readonly ICatProductRepository _catProductRepository;
    private readonly ILogger<WishlistRepository> _logger;
    public WishlistRepository(ApplicationDbContextFactory contextFactory, 
        ICatProductRepository catProductRepository,
        ILogger<WishlistRepository> logger)
    {
        _contextFactory = contextFactory;
        _catProductRepository = catProductRepository;
        _logger = logger;
    }

    public async Task<CatProduct[]> GetAsync(string userId, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();
        
        var wishlistCats = await context.WishlistCats.FromSqlInterpolated($"""
                                                 SELECT * 
                                                 FROM "WishlistCats"
                                                 WHERE "ApplicationUserId" = {userId}
                                                 """).ToArrayAsync(cancellationToken);
        var catProductIds = wishlistCats.Select(catProduct => catProduct.CatProductId).ToArray();
        var catProducts = await _catProductRepository.GetCatProductsByIdAsync(catProductIds, cancellationToken);
 
        return catProducts;
    }

    public async Task<bool> CheckCatInDbAsync(string userId, long catProductId, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();
        
        var inDb = await context.WishlistCats
            .FromSqlInterpolated($"""
                                  SELECT 1 
                                  FROM "WishlistCats" 
                                  WHERE "ApplicationUserId" = {userId} AND "CatProductId" = {catProductId}
                                  """)
            .AnyAsync(cancellationToken);

        return inDb;
    }
    
    public async Task<bool> AddAsync(string userId, long catProductId, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();

        var inDb = await CheckCatInDbAsync(userId, catProductId, cancellationToken);
        if (inDb)
        {
            return false;
        }

        try
        {
            await context.Database
                .ExecuteSqlInterpolatedAsync($"""
                                              INSERT INTO "WishlistCats" ("ApplicationUserId", "CatProductId") 
                                              VALUES ({userId}, {catProductId})
                                              """, 
                    cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(string userId, long catProductId, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();
        
        var result = await context.Database
            .ExecuteSqlInterpolatedAsync($"""
                                          DELETE FROM "WishlistCats" 
                                          WHERE "ApplicationUserId" = {userId} AND "CatProductId" = {catProductId}
                                          """, 
                cancellationToken);
    
        return result > 0;
    }
}