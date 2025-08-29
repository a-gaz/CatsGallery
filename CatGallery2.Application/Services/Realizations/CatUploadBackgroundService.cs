using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CatGallery2.Application.Services.Realizations;

public sealed class CatUploadBackgroundService : BackgroundService
{
    private readonly ICatImageUploadQueue _catImageUploadQueue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CatUploadBackgroundService> _logger;
    public CatUploadBackgroundService(ICatImageUploadQueue catImageUploadQueue, IServiceProvider serviceProvider, 
        ILogger<CatUploadBackgroundService> logger)
    {
        _catImageUploadQueue = catImageUploadQueue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var foreignId in _catImageUploadQueue.GetAll(stoppingToken))
        {
            using var scope = _serviceProvider.CreateScope();
            var catImageRepository = scope.ServiceProvider.GetRequiredService<ICatImageRepository>();
            var imageStorage = scope.ServiceProvider.GetRequiredService<IImageStorage>();
            var catProvider = scope.ServiceProvider.GetRequiredService<ICatProvider>();

            try
            {
                var imageIsAdded = await catImageRepository.CheckCatHasFileAsync(foreignId, stoppingToken);
                    
                if (!imageIsAdded)
                {
                    var image = await catProvider.GetImageByIdAsync(foreignId, stoppingToken);
                    var fileName = await imageStorage.UploadImageAsync(image, stoppingToken);
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