using CatGallery2.Application.Services.Entities;
using CatGallery2.Application.Services.Interfaces;

namespace CatGallery2.Infrastructure.PostgresStorage;

internal sealed class CatRepositoryStub : ICatRepository
{
    private readonly List<CatImage> _images = new List<CatImage>();
    private long _counter = 1;
    public Task<bool> AddCatAsync(string foreignId, CancellationToken cancellationToken)
    {
        var res = _images.Any(x => x.ForeignId == foreignId);
        if (res)
        {
            return Task.FromResult(false);
        }
        
        CatImage catImage = new CatImage
        {
            Id = ++_counter,
            ForeignId = foreignId,
        };
        _images.Add(catImage);
        
        return Task.FromResult(true);
    }

    public Task AddCatImageAsync(string foreignId, string fileName, CancellationToken cancellationToken)
    {
         var catImage = _images.Find(x => x.ForeignId == foreignId);
         if (catImage != null)
         {
             if (catImage.FileName == null)
             {
                 catImage.FileName = fileName;
                 catImage.UploadDate = DateTime.UtcNow;
             }
         }
         
         return Task.CompletedTask;
    }

    public Task<CatImage[]> GetCatsAsync(int pageSize, DateTime from, long[] viewedIds, CancellationToken cancellationToken)
    {
        return Task.FromResult(_images
            .Where(x => !viewedIds.Contains(x.Id))
            .OrderByDescending(x => x.UploadDate)
            .Take(pageSize)
            .ToArray());
    }

    public Task<bool> CheckCatHasFileAsync(string foreignId, CancellationToken stoppingToken)
    {
        return Task.FromResult(_images.Any(x => x.ForeignId == foreignId && x.FileName != null));
    }

    public Task<CatImage[]> GetCatsById(long[] viewedIds, CancellationToken cancellationToken)
    {
        var res = _images.Where(x => viewedIds.Contains(x.Id)).ToArray();
        return Task.FromResult(res); 
    }
}