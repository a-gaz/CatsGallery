namespace CatGallery2.Application.Services.Interfaces;

public interface IImageStorage
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>FileName</returns>
    Task<string> UploadImageAsync(Stream file, CancellationToken cancellationToken);
}