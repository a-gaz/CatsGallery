using System.Collections.Concurrent;
using System.Threading.Channels;
using CatsGallery.Application.Entities;
using CatsGallery.Application.Interfaces;
using CatsGallery.Application.Interfaces.Repos;
using CatsGallery.Infrastructure.Repos;
using CatsGallery.Shared.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatsGallery.Infrastructure.Services;

public class CatGalleryService : ICatGalleryService
{
    private readonly ICatApiService _catApiService;
    // private readonly ICachingService _cachingService;
    
    private readonly Settings _settings;
    private readonly ILogger<CatGalleryService> _logger;
    
    private readonly Channel<Cat> _processingChannel;
    private readonly CancellationTokenSource _cts = new();
    
    private readonly ConcurrentDictionary<string, IUserRepo> _userRepos = new();
    
    private readonly int _initialCatsNum = 3;
    private readonly int _catsNumInPage;
    
    public CatGalleryService(
        ICatApiService catApiService,
        // ICachingService cachingService,
        IOptions<Settings> settings,
        ILogger<CatGalleryService> logger)
    {
        _catApiService = catApiService;
        // _cachingService = cachingService;
        
        _settings = settings.Value;
        _logger = logger;
        
        _catsNumInPage = int.Parse(_settings.Util.CatsNumInPage);
        
        _processingChannel = Channel.CreateUnbounded<Cat>();
        
        _ = ProcessCatsBackgroundAsync(_cts.Token);
    }
    
    public void GetOrAddUserStorage(string userId)
    {
        _userRepos.GetOrAdd(userId, _ => new UserRepo());
    }

    public async Task<Cat[]> GetInitialCatsAsync(string userId)
    {
        if (!_userRepos.TryGetValue(userId, out var userRepo))
            throw new InvalidOperationException("НЕ ИНИЦИАЛИЗИРОВАН userRepo");

        if (userRepo.InitialCats.Count >= _initialCatsNum)
            return userRepo.InitialCats.Take(_initialCatsNum).ToArray();
        
        var cats = new List<Cat>();
        await foreach (var page in _catApiService.GetAllCatsPaginatedAsync(_catsNumInPage))
        {
            cats.AddRange(page);
            
            if (cats.Count >= _initialCatsNum) 
                break;
        }
        
        var initialCats = cats.Take(_initialCatsNum).ToArray();
        
        userRepo.InitialCats.AddRange(initialCats);
        
        foreach (var cat in initialCats)
        {
            await _processingChannel.Writer.WriteAsync(cat);
        }
        
        return initialCats;
    }
    
    public async Task<Cat> GetNextCatAsync(string userId)
    {
        if (!_userRepos.TryGetValue(userId, out var userRepo))
            throw new InvalidOperationException("НЕ ИНИЦИАЛИЗИРОВАН userRepo");
        
        if (userRepo.CatQueue.TryDequeue(out var cat))
            return cat;
        
        await foreach (var page in _catApiService.GetAllCatsPaginatedAsync(_catsNumInPage))
        {
            var newCat = page.FirstOrDefault();
            
            if (newCat != null)
            {
                await _processingChannel.Writer.WriteAsync(newCat);
                
                return newCat;
            }
        }
        
        throw new Exception("КОТОВ НЕТ!");
    }

    private async Task ProcessCatsBackgroundAsync(CancellationToken ct)
    {
        await foreach (var cat in _processingChannel.Reader.ReadAllAsync(ct))
        {
            // todo сохраняем в БД и кэш
            await Task.WhenAll(
                // _catRepo.AddAsync(cat),
                // _cachingService.CacheAsync(cat.Id, cat.ImageBytes)
            );
            
            _logger.LogInformation($"Id кота; {cat.Id}");
        }
    }
}