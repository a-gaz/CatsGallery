using System.Threading.Channels;
using CatGallery2.Application.Interfaces.Gateways;

namespace CatGallery2.Application.Realizations;

public class CatImageUploadQueue : ICatImageUploadQueue
{
    private readonly Channel<(string id, Stream catPhotoStream)> _channel;

    public CatImageUploadQueue()
    {
        var options = new UnboundedChannelOptions()
        {
            SingleReader = true,
            SingleWriter = false
        };
        _channel = Channel.CreateUnbounded<(string id, Stream catPhotoStream)>(options);
    }

    public async Task EnqueueAsync(string foreignId, Stream catPhotoStream, long newCatProductId,
        CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync((foreignId, catPhotoStream), cancellationToken);
    }

    public IAsyncEnumerable<(string id, Stream catPhotoStream)> GetAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}