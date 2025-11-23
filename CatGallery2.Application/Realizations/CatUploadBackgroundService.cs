using CatGallery2.Application.Interfaces.Gateways;
using CatGallery2.Application.Interfaces.Gateways.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CatGallery2.Application.Realizations;

public sealed class CatUploadBackgroundService : BackgroundService
{
    private readonly ICatImageRepository _catImageRepository;
    private readonly ICatImageUploadQueue _catImageUploadQueue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CatUploadBackgroundService> _logger;
    public CatUploadBackgroundService(ICatImageRepository catImageRepository,
        ICatImageUploadQueue catImageUploadQueue, 
        IServiceProvider serviceProvider, 
        ILogger<CatUploadBackgroundService> logger)
    {
        _catImageRepository = catImageRepository;
        _catImageUploadQueue = catImageUploadQueue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var catImageRepository = scope.ServiceProvider.GetRequiredService<ICatImageRepository>();
        var imageStorage = scope.ServiceProvider.GetRequiredService<IImageStorage>();
        
        await foreach (var (foreignId, catPhotoStream) in _catImageUploadQueue.GetAllAsync(stoppingToken))
        {
            try
            { 
                var imageIsAdded = await catImageRepository.CheckCatHasFileAsync(foreignId, stoppingToken);
                    
                if (!imageIsAdded)
                {
                    // var image = await catProvider.GetImageByIdAsync(foreignId, stoppingToken);
                    var fileName = await imageStorage.UploadImageAsync(catPhotoStream, stoppingToken);
                    await catImageRepository.AddCatImageAsync(foreignId, fileName, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обработке foreignId: {foreignId}");
            }
        }
    }
}