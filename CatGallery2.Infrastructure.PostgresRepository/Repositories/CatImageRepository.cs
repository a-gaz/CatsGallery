using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatGallery2.Infrastructure.PostgresRepository.Repositories;

internal sealed class CatImageRepository : ICatImageRepository
{
    private readonly ApplicationDbContextFactory _contextFactory;
    private readonly ILogger<CatImageRepository> _logger;
    
    public CatImageRepository(ApplicationDbContextFactory contextFactory, ILogger<CatImageRepository> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }
    
    public async Task<bool> TryAddCatAsync(CatImage catImage, long catProductId, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();
        
        var inDb = await context.CatImages
            .FromSqlInterpolated($"""
                                  SELECT 1 
                                  FROM "CatImages" 
                                  WHERE "ForeignId" = {catImage.ForeignId}
                                  """)
            .AnyAsync(cancellationToken);
        if (inDb)
        {
            return false;
        }
        
        await context.Database
            .ExecuteSqlInterpolatedAsync($"""
                                          INSERT INTO "CatImages" ("ForeignId",
                                                                   "CatProductId") 
                                          VALUES (
                                          {catImage.ForeignId}, 
                                          {catImage.CatProductId})
                                          """, 
            cancellationToken);
        
        return true;
    }

    public async Task AddCatImageAsync(string catImageForeignId, string fileName, CancellationToken cancellationToken)
    { 
        await using var context = _contextFactory.CreateDbContext();

        await context.Database
            .ExecuteSqlInterpolatedAsync($"""
                                          UPDATE "CatImages" 
                                          SET "FileName" = {fileName}
                                          WHERE "ForeignId" = {catImageForeignId}
                                          """, 
                cancellationToken);
    }

    public async Task<bool> CheckCatHasFileAsync(string catImageForeignId, CancellationToken stoppingToken)
    {
        await using var context = _contextFactory.CreateDbContext();

        var catImageIsExist = await context.CatImages
            .FromSqlInterpolated($"""
                                  SELECT 1 
                                  FROM "CatImages" 
                                  WHERE "ForeignId" = {catImageForeignId} AND 
                                  "FileName" != null 
                                  LIMIT 1
                                  """)
            .AnyAsync(stoppingToken);
        
        return catImageIsExist;
    }

    public async Task<CatImage> GetCatByFileNameAsync(string fileName, CancellationToken cancellationToken)
    {
        await using var context = _contextFactory.CreateDbContext();

        var catImage = await context.CatImages
            .FromSqlInterpolated($"""
                                  SELECT *
                                  FROM "CatImages" 
                                  WHERE "ForeignId" = {fileName}
                                  """)
            .SingleAsync(cancellationToken);
        
        if (catImage == null)
        {
            _logger.LogError($"CatImage with ForeignId={fileName} not found");
            throw new NullReferenceException();
        }
        
        return catImage;
    }
}