using CatGallery2.Application.Entities;

namespace CatGallery2.Application.Interfaces.Gateways.Repositories;

public interface ICatImageRepository
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="catImage">Объект кота</param>
    /// /// <param name="catProductId">внешний Id кота</param>
    /// <param name="cancellationToken"></param>
    /// <returns>true - добавлен в БД; false - уже есть в БД</returns>
    Task<bool> TryAddCatAsync(CatImage catImage, long catProductId, CancellationToken cancellationToken);
    Task AddCatImageAsync(string catImageForeignId, string fileName, CancellationToken cancellationToken);
    Task<bool> CheckCatHasFileAsync(string catImageForeignId, CancellationToken stoppingToken);
    Task<CatImage> GetCatByFileNameAsync(string fileName, CancellationToken cancellationToken);

}