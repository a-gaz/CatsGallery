using CatGallery2.Application.Services.Interfaces;

namespace CatGallery2.Infrastructure.Minio;

internal sealed class MinioImageStorageStub : IImageStorage
{
    public async Task<string> UploadImageAsync(Stream file, CancellationToken cancellationToken)
    {
        var fileName = Guid.NewGuid() + ".jpg";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", fileName);
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream, cancellationToken);
        }
        
        return fileName;
    }
}