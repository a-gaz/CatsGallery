using CatGallery2.Application.Interfaces.Gateways;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace CatGallery2.Infrastructure.MinioStorage;

internal sealed class MinioCacheImageStorage : IImageStorage
{
    private readonly string _bucketName;
    private readonly IMinioClient _client;

    public MinioCacheImageStorage(IMinioClient client, IOptions<MinioRepositoryOptions> options)
    {
        _client = client;
        
        _bucketName = options.Value.BucketName;
    }
    
    public async Task BucketExistsAsync(CancellationToken cancellationToken)
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

    public async Task<string> UploadImageAsync(Stream fileStream, CancellationToken cancellationToken)
    {
        var fileName = $"{Guid.NewGuid()}.jpg";
        
        await _client.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType("image/jpeg"), cancellationToken);

        return fileName;
    }

    public async Task<MemoryStream> DownloadImageAsync(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var stream = new MemoryStream();
            var tsc = new TaskCompletionSource<bool>();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithCallbackStream(cs =>
                {
                    cs.CopyTo(stream);
                    tsc.SetResult(true);
                });

            await _client.GetObjectAsync(getObjectArgs, cancellationToken);
            await tsc.Task;
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
     }
        catch (Exception)
        {
            throw;
        }
    }
}