using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace CatGallery2.Application.Services.Realizations;

public sealed class CatUploadBackgroundService : BackgroundService
{
    private readonly ICatImageUploadQueue _catImageUploadQueue;
    private readonly IImageStorage _imageStorage;
    private readonly ICatRepository _catRepository;
    private readonly ICatProvider _catProvider;

    public CatUploadBackgroundService(ICatRepository catRepository, IImageStorage imageStorage, ICatImageUploadQueue catImageUploadQueue, ICatProvider catProvider)
    {
        _catRepository = catRepository;
        _imageStorage = imageStorage;
        _catImageUploadQueue = catImageUploadQueue;
        _catProvider = catProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var foreignId in _catImageUploadQueue.GetAll(stoppingToken))
        {
            var imageIsAdded = await _catRepository.CheckCatHasFileAsync(foreignId, stoppingToken);
            if (!imageIsAdded)
            {
                var image = await _catProvider.GetImageByIdAsync(foreignId, stoppingToken);
                var fileName = await _imageStorage.UploadImageAsync(image, stoppingToken);
                
                await _catRepository.AddCatImageAsync(foreignId, fileName, stoppingToken);
            }
        }
    }
}