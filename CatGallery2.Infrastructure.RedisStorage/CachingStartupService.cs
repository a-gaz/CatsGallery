using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace CatGallery2.Infrastructure.RedisStorage;

public class CachingStartupService : IHostedService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IHostEnvironment _environment;
    
    public CachingStartupService(IConnectionMultiplexer connectionMultiplexer, IHostEnvironment environment)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _environment = environment;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_environment.IsDevelopment())
        {
            try
            {
                var endpoints = _connectionMultiplexer.GetEndPoints();
                var server = _connectionMultiplexer.GetServer(endpoints.First());
                await server.FlushDatabaseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА: {ex.Message}");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}