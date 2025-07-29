using CatGallery2.Application.Services.Entities;
using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CatGallery2.Application.Services.Realizations;

public sealed class CatService : ICatService
{
    private readonly ICatProvider _catProvider;
    private readonly ICatImageRepository _catImageRepository;
    private readonly IViewsRepository _viewsRepository;
    private readonly ICatImageUploadQueue _catImageUploadQueue;
    private readonly IImageStorage _cacheImageStorage;
    private readonly ILogger<CatService> _logger;
    
    public CatService(
        ICatProvider catProvider, 
        ICatImageRepository catImageRepository, 
        IViewsRepository viewsRepository, 
        ICatImageUploadQueue catImageUploadQueue,
        IImageStorage cacheImageStorage,
        ILogger<CatService> logger)
    {
        _catProvider = catProvider;
        _catImageRepository = catImageRepository;
        _viewsRepository = viewsRepository;
        _catImageUploadQueue = catImageUploadQueue;
        _cacheImageStorage = cacheImageStorage;
        _logger = logger;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await _cacheImageStorage.BucketExists(cancellationToken);
    }
    
    public async Task<CatImage[]> GetPrevCatsAsync(int catsNum, Guid userId, CancellationToken cancellationToken)
    {
        var prevCatsIds = await _viewsRepository.GetByUserAsync(userId, cancellationToken); 
        var currIndex = await _viewsRepository.GetUserCurrCatViewIndexAsync(userId, cancellationToken);
        
        if (prevCatsIds.Length == 0 || currIndex == -1)
        {
            throw new Exception("Нет котов!");
        }

        var newIndex = Math.Max(0, currIndex - catsNum);
        await _viewsRepository.SetUserCurrCatViewIndexAsync(userId, newIndex, cancellationToken);

        return await GetCatsAroundIndex(newIndex, prevCatsIds, cancellationToken);
    }

    public async Task<CatImage[]> GetNextCatsAsync(int catsNum, DateTime from, Guid userId, CancellationToken cancellationToken)
    {
        var newCats = await FetchNewImagesAsync(catsNum, from, userId, cancellationToken);
        
        await _viewsRepository.AddAsync(userId, newCats, cancellationToken);
        var catImageIds = await _viewsRepository.GetByUserAsync(userId, cancellationToken);
        
        var currIndex = await _viewsRepository.GetUserCurrCatViewIndexAsync(userId, cancellationToken);
        var newIndex = currIndex == -1 ? Math.Max(0, (newCats.Length - 1) - 1) : Math.Max(0, currIndex + catsNum);
        
        await _viewsRepository.SetUserCurrCatViewIndexAsync(userId, newIndex, cancellationToken);
        
        var cats = await GetCatsAroundIndex(newIndex, catImageIds, cancellationToken);

        WaitForAll(cats.ToArray());
        return cats;
    }
    
    private async Task<CatImage[]> FetchNewImagesAsync(int catsNum, DateTime from, Guid userId, CancellationToken cancellationToken)
    {
        var viewedCats = await _viewsRepository.GetByUserAsync(userId, cancellationToken);
        var catsFromDb = await _catImageRepository.GetCatsAsync(catsNum, from, viewedCats, cancellationToken);

        if (catsFromDb.Length < catsNum)
        {
            await LoadNewCatsAsync(cancellationToken);
            catsFromDb = await _catImageRepository.GetCatsAsync(catsNum, from, viewedCats, cancellationToken);
        }
        else
        {
            foreach (var cat in catsFromDb)
            {
                await _catImageUploadQueue.EnqueueAsync(cat.ForeignId, cancellationToken);
            }
        }
        
        return catsFromDb;
    }

    private async Task LoadNewCatsAsync(CancellationToken cancellationToken)
    {
        var catsFromApi = await _catProvider.GetRandomCatsOneByOneAsync(10, cancellationToken);
        
        foreach (var cat in catsFromApi)
        {
            var isAddedToDb = await _catImageRepository.AddCatAsync(cat.Id, cancellationToken);
            if(isAddedToDb)
            {
                await _catImageUploadQueue.EnqueueAsync(cat.Id, cancellationToken);
            }
        }
    }
    
    private void WaitForAll(CatImage[] catImages)
    {
        foreach (var catImage in catImages)
        {
            while (catImage.FileName == null)
            {
                Task.Delay(10);
            }
        }
    }

    public async Task<string> GetUrlAsync(string fileName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException("Файла нет!");
    
        return await _cacheImageStorage.GetPresignedUrlAsync(fileName, cancellationToken);
    }

    private async Task<CatImage[]> GetCatsAroundIndex(long currIndex, long[] viewedCatIds, CancellationToken cancellationToken)
    {
        var catIdsToFetch = new List<long>();

        var hasPrev = currIndex - 1 >= 0;
        var hasCurr = currIndex >= 0 && currIndex < viewedCatIds.Length;
        var hasNext = currIndex + 1 < viewedCatIds.Length;

        if (hasPrev)
        {
            catIdsToFetch.Add(viewedCatIds[currIndex - 1]);
        }
        if (hasCurr)
        {
            catIdsToFetch.Add(viewedCatIds[currIndex]);
        }
        if (hasNext)
        {
            catIdsToFetch.Add(viewedCatIds[currIndex + 1]);
        }
        
        var fetchedCats = await _catImageRepository.GetCatsById(catIdsToFetch.ToArray(), cancellationToken);
        CatImage FindCatById(long id) => fetchedCats.FirstOrDefault(c => c.Id == id);
        
        var prevCat = hasPrev ? FindCatById(viewedCatIds[currIndex - 1]) : null;
        var currCat = hasCurr ? FindCatById(viewedCatIds[currIndex]) : null;
        var nextCat = hasNext ? FindCatById(viewedCatIds[currIndex + 1]) : null;
        
        return new [] { prevCat, currCat, nextCat };
    }
}