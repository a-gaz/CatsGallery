namespace CatGallery2.Application.Services.Interfaces;

public interface ICatImageUploadQueue
{
    /// <summary>
    /// Синглтон
    /// Добавляет картинку к коту
    /// </summary>
    /// <param name="foreignCatId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task EnqueueAsync(string foreignCatId, CancellationToken cancellationToken);

    IAsyncEnumerable<string> GetAll(CancellationToken cancellationToken);
}