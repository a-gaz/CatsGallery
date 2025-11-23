namespace CatGallery2.Application.Interfaces.Gateways;

public interface ICatImageUploadQueue
{
    /// <summary>
    /// Синглтон
    /// Добавляет картинку к коту
    /// </summary>
    /// <param name="foreignId"></param>
    /// <param name="catPhotoStream"></param>
    /// <param name="newCatProductId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task EnqueueAsync(string foreignId, Stream catPhotoStream, long newCatProductId,
        CancellationToken cancellationToken);

    IAsyncEnumerable<(string id, Stream catPhotoStream)> GetAllAsync(CancellationToken cancellationToken);
}