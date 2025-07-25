namespace CatGallery2.Application.Services.Interfaces;

public interface IImageStorage
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>FileName</returns>
    Task BucketExists(CancellationToken cancellationToken);
    Task<string> UploadImageAsync(Stream fileStream, CancellationToken cancellationToken);
    Task<string> GetPresignedUrlAsync(string fileName, CancellationToken cancellationToken);
    Task DownloadImageAsync(string fileName, Stream outputStream, CancellationToken cancellationToken);
}