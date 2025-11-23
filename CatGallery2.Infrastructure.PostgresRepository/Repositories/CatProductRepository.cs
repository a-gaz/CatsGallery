using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatGallery2.Infrastructure.PostgresRepository.Repositories;

internal class CatProductRepository : ICatProductRepository
{
    private readonly ApplicationDbContextFactory _contextFactory;
    private readonly ILogger<CatProductRepository> _logger;
    
    public CatProductRepository(ApplicationDbContextFactory contextFactory, ILogger<CatProductRepository> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }
    
    public async Task<long> CreateAsync(CatProduct catProduct, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();
     
        var catProductId = await CreateCatProductObjAsync(catProduct, cancellationToken);
        var createdCatProduct = await GetCatProductByIdAsync(catProductId, cancellationToken);
        createdCatProduct.UpdateTagProductIds(catProductId);
        
        // todo пока не удалять
        // if (catProduct.TagProducts?.Count > 0)
        // {
        //     await ProcessTagProductsAsync(context, catProductId, catProduct.TagProducts);
        // }
        
        return catProductId;
    }

    private async Task<long> CreateCatProductObjAsync(CatProduct catProduct, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();
        var catProductId = await CreateCatProductAsync(context, catProduct, cancellationToken);
        return catProductId;
    }

    public Task<bool> DeleteAsync(long catProductId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CatProductInDb(long catProductId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

     public async Task<CatProduct[]> GetCatProductsByIdAsync(long[] catPriductIds, CancellationToken cancellationToken)
     {
         await using var context = _contextFactory.CreateDbContext();
         
         var catProducts = await context.CatProducts
             .Where(cp => catPriductIds.Contains(cp.Id))
             .OrderByDescending(cp => cp.UploadDate)
             .Include(cp => cp.CatImages)
             .AsSplitQuery()
             .AsNoTracking()
             .ToArrayAsync(cancellationToken);
         
         return catProducts;
     }

     private async Task<CatProduct> GetCatProductByIdAsync(long catProductId, CancellationToken cancellationToken)
     {
         await using var context = _contextFactory.CreateDbContext();
         
         var catProduct = await context.CatProducts
             .Where(cp => cp.Id == catProductId)
             .OrderByDescending(cp => cp.UploadDate)
             .Include(cp => cp.CatImages)
             .AsSplitQuery()
             .AsNoTracking()
             .FirstOrDefaultAsync(cancellationToken);
         //
         // if (catProduct == null)
         // {
         //     throw new NullReferenceException();
         // }
         //
         return catProduct;
     }
    public async Task<CatProduct[]> GetCatsAsync(int pageSize, DateTime from, long[] viewedIds, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();

        var catProducts = await context.CatProducts
            .Where(cp => !viewedIds.Contains(cp.Id) && cp.UploadDate >= from)
            // .OrderByDescending(cp => cp.UploadDate)
            .Take(pageSize)
            .Include(cp => cp.CatImages)
            .AsSplitQuery()
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
        
        return catProducts;
    }
    
    private async Task<long> CreateCatProductAsync(ApplicationDbContext context, CatProduct catProduct, CancellationToken cancellationToken)
    {
        try
        {
            context.CatProducts.Add(catProduct);
            await context.SaveChangesAsync(cancellationToken);
        
            return catProduct.Id;
        }
        catch (Exception e)
        {
            throw new Exception($"CatProduct {catProduct.Name} не создан: {e}");
        }
    }

    private async Task ProcessTagProductsAsync(ApplicationDbContext context, long catProductId, ICollection<TagProduct> tagProducts)
    {
        foreach (var tagProduct in tagProducts)
        {
            var tagId = await GetOrCreateTagAsync(context, tagProduct.Tag);
            await CreateTagProductRelationAsync(context, catProductId, tagId);
        }
    }
    
    private async Task<int> GetOrCreateTagAsync(ApplicationDbContext context, Tag tag)
    {
        var checkSql = $"""
                        SELECT Id 
                        FROM "Tags" 
                        WHERE Name = {0}
                        """;
        
        var existingTag = await context.Tags
            .FromSqlRaw(checkSql, tag.Name)
            .FirstOrDefaultAsync();
        
        if (existingTag == null)
        {
            var insertSql = $"""
                             INSERT INTO "Tags" (Name) OUTPUT INSERTED.Id 
                             VALUES ({0})
                             """;
            var newTagId = await context.Database
                .SqlQueryRaw<int>(insertSql, tag.Name)
                .FirstOrDefaultAsync();
            return newTagId;
        }
        else
        {
            return tag.Id;
        }
    }

    private async Task CreateTagProductRelationAsync(ApplicationDbContext context, long catProductId, int tagId)
    {
        var sql = """
                  INSERT INTO "TagProduct" ("TagId", "CatProductId")
                  VALUES ({0}, {1})
                  ON CONFLICT ("TagId", "CatProductId") DO NOTHING
                  """;
    
        await context.Database.ExecuteSqlRawAsync(sql, tagId, catProductId);
    }
}