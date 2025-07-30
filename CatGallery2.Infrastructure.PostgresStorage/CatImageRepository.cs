using CatGallery2.Application.Services.Entities;
using CatGallery2.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatGallery2.Infrastructure.PostgresStorage;

internal sealed class CatImageRepository : ICatImageRepository
{
    protected readonly ApplicationDbContext _context;
    private static long _counter = 0;
    
    public CatImageRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> AddCatAsync(string foreignId, CancellationToken cancellationToken)
    {
        try
        {
            if (await _context.CatImages.AnyAsync(x => x.ForeignId == foreignId, cancellationToken))
                return false;

            var catImage = new CatImage { ForeignId = foreignId };
            _context.CatImages.Add(catImage);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task AddCatImageAsync(string foreignId, string fileName, CancellationToken cancellationToken)
    {
        var catImage = await _context.CatImages.FirstOrDefaultAsync(x => x.ForeignId == foreignId, cancellationToken);
        if (catImage != null)
        {
            if (catImage.FileName == null)
            {
                catImage.FileName = fileName;
                catImage.UploadDate = DateTime.UtcNow;
                
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public async Task<CatImage[]> GetCatsAsync(int pageSize, DateTime from, long[] viewedIds, CancellationToken cancellationToken)
    {
        return await _context.CatImages.Where(x => !viewedIds.Contains(x.Id))
            .OrderByDescending(x => x.UploadDate)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<bool> CheckCatHasFileAsync(string foreignId, CancellationToken stoppingToken)
    {
        var res = _context.CatImages.Any(x => x.ForeignId == foreignId && x.FileName != null);
        return res;
    }

    public async Task<CatImage[]> GetCatsById(long[] viewedIds, CancellationToken cancellationToken)
    {
        var res = await _context.CatImages.Where(x => viewedIds.Contains(x.Id)).ToArrayAsync(cancellationToken);
        return res;
    }
}