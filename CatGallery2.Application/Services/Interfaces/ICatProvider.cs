namespace CatGallery2.Application.Services.Interfaces;

public interface ICatProvider
{
    Task<CatResponse[]> GetRandomCatsOneByOneAsync(int pageSize, CancellationToken cancellationToken);
    Task<CatResponse[]> GetRandomCatsAsync(int pageSize, CancellationToken cancellationToken);
    Task<Stream> GetImageByIdAsync(string id, CancellationToken cancellationToken);
}

public sealed record CatResponse(string Id);
