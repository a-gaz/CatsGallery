namespace CatGallery2.Application.Interfaces.Gateways;

public interface IImageStorage
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>FileName</returns>
    Task BucketExistsAsync(CancellationToken cancellationToken);
    Task<string> UploadImageAsync(Stream fileStream, CancellationToken cancellationToken);
    Task<MemoryStream> DownloadImageAsync(string fileName, CancellationToken cancellationToken);
}