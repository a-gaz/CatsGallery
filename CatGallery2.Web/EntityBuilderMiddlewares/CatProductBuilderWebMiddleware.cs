using CatGallery2.Application;
using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Gateways;
using CatGallery2.Application.Interfaces.Gateways.Repositories;
using CatGallery2.Web.ViewModels;

namespace CatGallery2.Web.EntityBuilderMiddlewares;

public class CatProductBuilderWebMiddleware : ICatProductBuilderWebMiddleware
{
    private readonly ICatProductRepository _catProductRepository;
    private readonly ICatImageUploadQueue _catImageUploadQueue;
    private readonly ICatImageRepository _catImageRepository;
    public CatProductBuilderWebMiddleware(ICatProductRepository catProductRepository, 
        ICatImageUploadQueue catImageUploadQueue, 
        ICatImageRepository catImageRepository)
    {
        _catProductRepository = catProductRepository;
        _catImageUploadQueue = catImageUploadQueue;
        _catImageRepository = catImageRepository;
    }

    public async Task CreateCatProductAsync(CatProductCreateViewModel model, 
        ApplicationUser? currentUser,
        CancellationToken cancellationToken)
    {
        if (currentUser == null)
        {
            throw new ArgumentNullException(nameof(currentUser));
        }
        
        var catProduct = new CatProduct()
        {
            Name = model.Name,
            Description = model.Description,
            UploadDate = DateTime.UtcNow,
            Price = model.Price,
            
            
            Source = CatProductSource.Web,
            ApplicationUserId = currentUser.Id
            
        };
        
        // todo CatImages
        // todo TagProducts
        
        var newCatProductId = await _catProductRepository.CreateAsync(catProduct, cancellationToken);
               
        var tagProducts = new List<TagProduct>();
        foreach (var tag in model.Tags)
        {
            var tagProduct = new TagProduct()
            {
                Tag = new Tag() { Name = tag },
                CatProductId = newCatProductId
            };
            
            tagProducts.Add(tagProduct);
        }
        
        var streams = FormFilesToStreams(model.CatPhotos);
        var catImageForeignIds = await PushCatImageStreamsToQueue(streams, newCatProductId, cancellationToken);

        // await _catImageRepository.AddCatImagesToCatProductAsync(newCatProductId, catImageForeignIds, cancellationToken);

        // todo продолжение
    }

    private List<Stream> FormFilesToStreams(List<IFormFile> formFiles)
    {
        var streams = new List<Stream>();
        foreach (var photo in formFiles)
        {
            streams.Add(photo.OpenReadStream());
        }

        return streams;
    }

    private async Task<List<string>> PushCatImageStreamsToQueue(List<Stream> catImageStreams, 
        long newCatProductId,
        CancellationToken cancellationToken)
    {
        var catImageIds = new List<string>();
        foreach (var stream in catImageStreams)
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
                await _catImageUploadQueue.EnqueueAsync(catImageForeignId, stream, newCatProductId, cancellationToken);
                
                catImageIds.Add(catImageForeignId);
            }
        }
        
        return catImageIds;
    }
}