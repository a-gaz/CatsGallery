using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Gateways.Repositories;
using CatGallery2.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace CatGallery2.Application.Realizations.Services;

public class WishlistService : IWishlistService
{
    private readonly ICatImageRepository _catImageRepository;
    private readonly IWishlistRepository _wishlistRepository;
    private readonly ILogger<WishlistService> _logger;

    public WishlistService(ICatImageRepository catImageRepository, IWishlistRepository wishlistRepository, ILogger<WishlistService> logger)
    {
        _catImageRepository = catImageRepository;
        _wishlistRepository = wishlistRepository;
        _logger = logger;
    }
    
    public async Task<bool> AddAsync(string userId, string foreignId, CancellationToken cancellationToken)
    {
        var catImage = await _catImageRepository.GetCatByFileNameAsync(foreignId, cancellationToken);

        return await _wishlistRepository.AddAsync(userId, catImage.CatProductId, cancellationToken);
    }

    public async Task<bool> DeleteAsync(string userId, string foreignId, CancellationToken cancellationToken)
    {
        var catImage = await _catImageRepository.GetCatByFileNameAsync(foreignId, cancellationToken);
        
        return await _wishlistRepository.DeleteAsync(userId, catImage.CatProductId, cancellationToken);
    }

    public async Task<CatProduct[]> GetAsync(string currentUserId, CancellationToken cancellationToken)
    {
        return await _wishlistRepository.GetAsync(currentUserId, cancellationToken);
    }
}