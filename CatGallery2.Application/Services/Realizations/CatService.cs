using CatGallery2.Application.Services.Entities;
using CatGallery2.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CatGallery2.Application.Services.Realizations;

public sealed class CatService : ICatService
{
    private readonly ICatProvider _catProvider;
    private readonly ICatRepository _catRepository;
    private readonly IViewsRepository _viewsRepository;
    private readonly ICatImageUploadQueue _catImageUploadQueue;
    private readonly ILogger<CatService> _logger;
    
    private List<CatImage> _catImages = new List<CatImage>();
    private int _idx = 0;
    public CatService(ICatProvider catProvider, ICatRepository catRepository, IViewsRepository viewsRepository, 
        ICatImageUploadQueue catImageUploadQueue, ILogger<CatService> logger)
    {
        _catProvider = catProvider;
        _catRepository = catRepository;
        _viewsRepository = viewsRepository;
        _catImageUploadQueue = catImageUploadQueue;
        _logger = logger;
    }

    
    public async Task<CatImage[]> GetPrevCatsAsync(int catsNum, Guid userId, CancellationToken cancellationToken)
    {
        var prevIdx = _idx - 2;
        if (prevIdx <= 0)
        {
            throw new Exception("Это конец!");
        }
        
        if (_catImages.Count >= catsNum)
        {
            _catImages = _catImages.Take(_catImages.Count - catsNum).ToList();
        }
        
        var prevCatsIds = (await _viewsRepository.GetByUserAsync(userId, cancellationToken))[prevIdx];
        
        var prevCats = await _catRepository.GetCatsById([prevCatsIds], cancellationToken);

        _catImages.InsertRange(0, prevCats);

        --_idx;
        return _catImages.ToArray();
    }
    
    public async Task<CatImage[]> GetNextCatsAsync(int catsNum, DateTime from, Guid userId, CancellationToken cancellationToken)
    {
        if (_catImages.Count >= catsNum)
        {
            _catImages = _catImages.TakeLast(_catImages.Count - catsNum).ToList();
        }
        
        var newCats = await FetchNewImagesAsync(catsNum, from, userId, cancellationToken);
        
        await RegisterViewsAsync(userId, newCats, cancellationToken);
        
        _catImages.AddRange(newCats);

        ++_idx;
        return _catImages.ToArray();
    }

    private async Task<CatImage[]> FetchNewImagesAsync(int catsNum, DateTime from, Guid userId, CancellationToken cancellationToken)
    {
        // к базе
        var viewedCats = await _viewsRepository.GetByUserAsync(userId, cancellationToken);
        var cats = await _catRepository.GetCatsAsync(catsNum, from, viewedCats, cancellationToken);
        // к api
        if (cats.Length < catsNum)
        {
            await LoadNewCatsAsync(cancellationToken);
            cats = await _catRepository.GetCatsAsync(catsNum, from, viewedCats, cancellationToken);
        }

        return cats;
    }

    private async Task LoadNewCatsAsync(CancellationToken cancellationToken)
    {
        var newCats = await _catProvider.GetRandomCatsOneByOneAsync(10, cancellationToken);
        
        foreach (var newCat in newCats)
        {
            var isAddedToDb = await _catRepository.AddCatAsync(newCat.Id, cancellationToken);
            if(isAddedToDb)
            {
                await _catImageUploadQueue.EnqueueAsync(newCat.Id, cancellationToken);
            }
        }
    }
    
    private async Task RegisterViewsAsync(Guid userId, IEnumerable<CatImage> newCats, CancellationToken cancellationToken)
    {
        foreach (var cat in newCats)
        {
            var viewedCat = new UserViewedCat(userId, cat.Id);
            await _viewsRepository.AddAsync(viewedCat, cancellationToken);
        }
    }
}