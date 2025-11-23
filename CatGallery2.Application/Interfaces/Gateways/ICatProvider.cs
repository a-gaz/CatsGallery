namespace CatGallery2.Application.Interfaces.Gateways;

public interface ICatProvider
{
    Task<IReadOnlyCollection<CatResponse>> GetRandomCatsOneByOneAsync(int pageSize, CancellationToken cancellationToken);
    Task<Stream> GetImageByIdAsync(string id, CancellationToken cancellationToken);
}

public sealed record CatResponse(string Id, List<string> Tags);
