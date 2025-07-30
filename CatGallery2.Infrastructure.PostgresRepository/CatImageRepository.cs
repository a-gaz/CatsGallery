using CatGallery2.Application.Services.Entities;
using CatGallery2.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatGallery2.Infrastructure.PostgresRepository;

internal sealed class CatImageRepository : ICatImageRepository
{
    private readonly ApplicationDbContextFactory _contextFactory;
    
    public CatImageRepository(ApplicationDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    public async Task<bool> TryAddCatAsync(string foreignId, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();
        
        var inDb = await context.CatImages
            .FromSqlInterpolated($"""
                                  SELECT 1 
                                  FROM "CatImages" 
                                  WHERE "ForeignId" = {foreignId}
                                  """)
            .AnyAsync(cancellationToken);
        if (inDb)
        {
            return false;
        }
        
        await context.Database
            .ExecuteSqlInterpolatedAsync($"""
                                          INSERT INTO "CatImages" ("ForeignId") 
                                          VALUES ({foreignId})
                                          """, 
            cancellationToken);
        
        return true;
    }

    public async Task AddCatImageAsync(string foreignId, string fileName, CancellationToken cancellationToken)
    { 
        await using var context = _contextFactory.CreateDbContext();

        await context.Database
            .ExecuteSqlInterpolatedAsync($"""
                                          UPDATE "CatImages" 
                                          SET "FileName" = {fileName}, 
                                          "UploadDate" = {DateTime.UtcNow} 
                                          WHERE "ForeignId" = {foreignId}
                                          """, 
                cancellationToken);
    }

    public async Task<CatImage[]> GetCatsAsync(int pageSize, DateTime from, long[] viewedIds, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();

        var catImages = await context.CatImages
            .FromSqlInterpolated($"""
                                  SELECT * 
                                  FROM "CatImages" 
                                  WHERE NOT ("Id" = ANY({viewedIds}))
                                  ORDER BY "UploadDate" DESC
                                  LIMIT {pageSize}
                                  """)
            .ToArrayAsync(cancellationToken);
        
        return catImages;
    }

    public async Task<bool> CheckCatHasFileAsync(string foreignId, CancellationToken stoppingToken)
    {
        await using var context = _contextFactory.CreateDbContext();

        var catImageIsExist = await context.CatImages
            .FromSqlInterpolated($"""
                                  SELECT 1 
                                  FROM "CatImages" 
                                  WHERE "ForeignId" = {foreignId} AND 
                                  "FileName" != null 
                                  LIMIT 1
                                  """)
            .AnyAsync(stoppingToken);
        
        return catImageIsExist;
    }

    public async Task<CatImage[]> GetCatsById(long[] viewedIds, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();

        var catImages = await context.CatImages
            .FromSqlInterpolated($"""
                                  SELECT * 
                                  FROM "CatImages" 
                                  WHERE "Id" = ANY({viewedIds})
                                  """)
            .ToArrayAsync(cancellationToken);
        
        return catImages;
    }
}