using CatGallery2.Application.Services.Entities;

namespace CatGallery2.Application.Services.Interfaces;

public interface ICatRepository
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="foreignId">внешний Id кота</param>
    /// <param name="cancellationToken"></param>
    /// <returns>true - добавлен в БД; false - уже есть в БД</returns>
    Task<bool> AddCatAsync(string foreignId, CancellationToken cancellationToken);
    Task AddCatImageAsync(string foreignId, string fileName, CancellationToken cancellationToken);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageSize">загружает готов кв кол-ве до pageSize</param>
    /// <param name="from"></param>
    /// <param name="viewedIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CatImage[]> GetCatsAsync(int pageSize, DateTime from, long[] viewedIds, CancellationToken cancellationToken);

    Task<bool> CheckCatHasFileAsync(string foreignId, CancellationToken stoppingToken);
    
    Task<CatImage[]> GetCatsById(long[] viewedIds, CancellationToken cancellationToken);
}