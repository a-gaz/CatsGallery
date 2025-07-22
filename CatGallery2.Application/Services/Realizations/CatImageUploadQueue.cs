using System.Threading.Channels;
using CatGallery2.Application.Services.Interfaces;

namespace CatGallery2.Application.Services.Realizations;

public class CatImageUploadQueue : ICatImageUploadQueue
{
    private readonly Channel<string> _channel;

    public CatImageUploadQueue()
    {
        var options = new UnboundedChannelOptions()
        {
            SingleReader = true,
            SingleWriter = false
        };
        _channel = Channel.CreateUnbounded<string>(options);
    }
    public async Task EnqueueAsync(string foreignCatId, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(foreignCatId, cancellationToken);
    }

    public IAsyncEnumerable<string> GetAll(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}