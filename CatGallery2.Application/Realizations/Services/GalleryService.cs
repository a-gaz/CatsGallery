using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.EntityBuilders;
using CatGallery2.Application.Interfaces.Gateways;
using CatGallery2.Application.Interfaces.Gateways.Repositories;
using CatGallery2.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace CatGallery2.Application.Realizations.Services;

public sealed class GalleryService : IGalleryService
{
    private readonly ICatProductRepository _catProductRepository;
    private readonly IUserActivityRepository _userActivityRepository;
    private readonly ICatImageUploadQueue _catImageUploadQueue;
    private readonly IImageStorage _cacheImageStorage;
    private readonly IWishlistRepository _wishlistRepository;
    private readonly ICatProductBuilder _catProductBuilder;
    private readonly ILogger<GalleryService> _logger;
    
    public GalleryService(
        ICatProductRepository catProductRepository, 
        IUserActivityRepository userActivityRepository,
        ICatImageUploadQueue catImageUploadQueue,
        IImageStorage cacheImageStorage,
        IWishlistRepository wishlistRepository,
        ICatProductBuilder catProductBuilder,
        ILogger<GalleryService> logger)
    {
        _catProductRepository = catProductRepository;
        _userActivityRepository = userActivityRepository;
        _cacheImageStorage = cacheImageStorage;
        _wishlistRepository = wishlistRepository;
        _catProductBuilder = catProductBuilder;
        _catImageUploadQueue = catImageUploadQueue;
        _logger = logger;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await _cacheImageStorage.BucketExistsAsync(cancellationToken);
    }
    
    public async Task<CatProduct[]> GetPrevCatsAsync(int catsNum, Guid userId, CancellationToken cancellationToken)
    {
        var prevCatsIds = await _userActivityRepository.GetByUserAsync(userId, cancellationToken); 
        var currIndex = await _userActivityRepository.GetUserCurrCatViewIndexAsync(userId, cancellationToken);
        
        if (prevCatsIds.Length == 0 || currIndex == -1)
        {
            throw new Exception("Нет котов!");
        }

        var newIndex = Math.Max(0, currIndex - catsNum);
        await _userActivityRepository.SetUserCurrCatViewIndexAsync(userId, newIndex, cancellationToken);

        return await GetCatsAroundIndex(newIndex, prevCatsIds, cancellationToken);
    }

    public async Task<CatProduct[]> GetNextCatsAsync(int catsNum, DateTime from, Guid userId, CancellationToken cancellationToken)
    {
        var newCats = await FetchNewImagesAsync(catsNum, from, userId, cancellationToken);
        
        await _userActivityRepository.AddAsync(userId, newCats, cancellationToken);
        var catProductIds = await _userActivityRepository.GetByUserAsync(userId, cancellationToken);
        
        var currIndex = await _userActivityRepository.GetUserCurrCatViewIndexAsync(userId, cancellationToken);
        var newIndex = currIndex == -1 ? Math.Max(0, (newCats.Length - 1) - 1) : Math.Max(0, currIndex + catsNum);
        
        await _userActivityRepository.SetUserCurrCatViewIndexAsync(userId, newIndex, cancellationToken);
        
        var catProducts = await GetCatsAroundIndex(newIndex, catProductIds, cancellationToken);
        
        return catProducts;
    }
    
    public async Task<byte[]> GetCatImageBytesAsync(string fileName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException("Файла нет!");

        var catImageMemStream = await _cacheImageStorage.DownloadImageAsync(fileName, cancellationToken);
        return catImageMemStream.ToArray();
    }

    public async Task<bool[]> CheckWishlistAsync(string currentUserId, CatProduct[] catImages, CancellationToken cancellationToken)
    {
        var catsInWishlist = new bool[catImages.Length];

        for (var i = 0; i < catImages.Length; i++)
        {
            catsInWishlist[i] = await _wishlistRepository.CheckCatInDbAsync(currentUserId, catImages[i].Id, cancellationToken);
        }
        
        return catsInWishlist;
    }

    private async Task<CatProduct[]> FetchNewImagesAsync(int catsNum, DateTime from, Guid userId, CancellationToken cancellationToken)
    {
        var viewedCats = await _userActivityRepository.GetByUserAsync(userId, cancellationToken);
        var catsFromDb = await _catProductRepository.GetCatsAsync(catsNum, from, viewedCats, cancellationToken);

        if (catsFromDb.Length < catsNum)
        {
            await _catProductBuilder.LoadNewCatsAsync(cancellationToken);
            catsFromDb = await _catProductRepository.GetCatsAsync(catsNum, from, viewedCats, cancellationToken);
        }
        
        return catsFromDb;
    }
    
    private async Task<CatProduct[]> GetCatsAroundIndex(long currIndex, long[] viewedCatIds, CancellationToken cancellationToken)
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
        
        var fetchedCats = await _catProductRepository.GetCatProductsByIdAsync(catIdsToFetch.ToArray(), cancellationToken);
        CatProduct FindCatById(long id) => fetchedCats.FirstOrDefault(c => c.Id == id);
        
        var prevCat = hasPrev ? FindCatById(viewedCatIds[currIndex - 1]) : null;
        var currCat = hasCurr ? FindCatById(viewedCatIds[currIndex]) : null;
        var nextCat = hasNext ? FindCatById(viewedCatIds[currIndex + 1]) : null;
        
        return new [] { prevCat, currCat, nextCat };
    }
}