using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace CatGallery2.Infrastructure.Minio;

internal sealed class MinioCacheImageStorage : IImageStorage
{
    private readonly string _bucketName;
    private readonly IMinioClient _client;

    public MinioCacheImageStorage(IMinioClient client, IOptions<MinioRepositoryOptions> options)
    {
        _client = client;
        
        _bucketName = options.Value.BucketName;
    }
    
    public async Task BucketExists(CancellationToken cancellationToken)
    {
        try
        {
            var exists = await _client.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(_bucketName), 
                cancellationToken);
            
            if (!exists)
            {
                await _client.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(_bucketName), 
                    cancellationToken);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> UploadImageAsync(Stream fileStream, CancellationToken cancellationToken)
    {
        try
        {
            var fileName = $"{Guid.NewGuid()}.jpg";
            
            await _client.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType("image/jpeg"));

            return fileName;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public async Task<string> GetPresignedUrlAsync(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithExpiry(3600);

            string url = await _client.PresignedGetObjectAsync(args);
            return url;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DownloadImageAsync(string fileName, Stream outputStream, CancellationToken cancellationToken)
    {
        try
        {
            var statArgs = new StatObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName);
        
            await _client.StatObjectAsync(statArgs, cancellationToken);
    
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream => 
                {
                    stream.CopyTo(outputStream);
                    outputStream.Seek(0, SeekOrigin.Begin);
                });
    
            await _client.GetObjectAsync(getObjectArgs, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }
}