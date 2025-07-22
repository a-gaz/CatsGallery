using CatGallery2.Application.Services.Entities;

namespace CatGallery2.Application.Services.Interfaces;

public interface ICatService
{
    Task<CatImage[]> GetNextCatsAsync(int catsNum, DateTime from, Guid userId, CancellationToken cancellationToken);
}