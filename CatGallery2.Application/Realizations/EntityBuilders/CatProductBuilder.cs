using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.EntityBuilders;
using CatGallery2.Application.Interfaces.Gateways;
using CatGallery2.Application.Interfaces.Gateways.Repositories;

namespace CatGallery2.Application.Realizations.EntityBuilders;

public class CatProductBuilder : ICatProductBuilder
{
    private readonly ICatProvider _catProvider;
    private readonly ICatImageRepository _catImageRepository;
    private readonly ICatProductRepository _catProductRepository;
    private readonly ICatImageUploadQueue _catImageUploadQueue;
    private readonly IAuthService _authService;
    public CatProductBuilder(ICatProvider catProvider, 
        ICatImageRepository catImageRepository,
        ICatProductRepository catProductRepository,
        ICatImageUploadQueue catImageUploadQueue,
        IAuthService authService)
    {
        _catProvider = catProvider;
        _catImageRepository = catImageRepository;
        _catProductRepository = catProductRepository;
        _catImageUploadQueue = catImageUploadQueue;
        _authService = authService;
    }

    public async Task LoadNewCatsAsync(CancellationToken cancellationToken)
    {
        var catResponses = await _catProvider.GetRandomCatsOneByOneAsync(10, cancellationToken);
        
        foreach (var catResponse in catResponses)
        {
            var catName = "Заглушка генерации имени";
            var catDescription = "Заглушка генерации имени. Заглушка генерации имени. Заглушка генерации имени. Заглушка генерации имени. Заглушка генерации имени. Заглушка генерации имени. Заглушка генерации имени. Заглушка генерации имени. Заглушка генерации имени. Заглушка генерации имени. Заглушка генерации имени.";
            var catPrice = 1;
            var catTags = catResponse.Tags;
            var stream = await _catProvider.GetImageByIdAsync(catResponse.Id, cancellationToken);
            var applicationUser = await _authService.GetUserAdminAsync(cancellationToken);
            var catProduct = CreateCatProductAsync(catName, 
                catDescription, 
                catPrice, 
                catTags,
                CatProductSource.Api, 
                applicationUser.Id);

            var newCatProductId = await _catProductRepository.CreateAsync(catProduct, cancellationToken);
            
            var catImageForeignIds = await PushCatImageStreamsToQueue([stream], newCatProductId, cancellationToken);
        }
    }

    private CatProduct CreateCatProductAsync(string catName, 
        string catDescription, 
        decimal catPrice, 
        List<string> catTags,
        CatProductSource source,
        string applicationUserId)
    {
        var tagProducts = CreateTagProducts(catTags);
        
        var catProduct = new CatProduct()
        {
            Name = catName,
            Description = catDescription,
            UploadDate = DateTime.UtcNow,
            Price = catPrice,
            TagProducts = tagProducts,
            Source = source,
            ApplicationUserId = applicationUserId
        };
        
        return catProduct;
    }

    private async Task<List<string>> PushCatImageStreamsToQueue(List<Stream> catImageStreams, 
        long newCatProductId, 
        CancellationToken cancellationToken)
    {
        var catImageIds = new List<string>();
        foreach (var photo in catImageStreams)
        {
            var catImageForeignId = Guid.NewGuid().ToString();
            var catImage = new CatImage()
            {
                ForeignId = catImageForeignId,
                CatProductId = newCatProductId
            };
            
            var isAddedToDb = await _catImageRepository.TryAddCatAsync(catImage, newCatProductId, cancellationToken);
            if (isAddedToDb)
            { 
                await _catImageUploadQueue.EnqueueAsync(catImageForeignId, photo, newCatProductId, cancellationToken);
                
                catImageIds.Add(catImageForeignId);
            }
        }
        
        return catImageIds;
    }
    
    private List<TagProduct> CreateTagProducts(List<string> catTags)
    {
        var tagProducts = new List<TagProduct>();
        foreach (var tag in catTags)
        {
            var tagProduct = new TagProduct()
            {
                Tag = new Tag() { Name = tag },
            };
            
            tagProducts.Add(tagProduct);
        }
        
        return tagProducts;
    }
}