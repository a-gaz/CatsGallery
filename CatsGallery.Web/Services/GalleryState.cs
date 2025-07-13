using CatsGallery.Application;
using CatsGallery.Application.Models;

namespace CatsGallery.Web.Services;

public class GalleryState : IGalleryState
{
    private ICatService _catService;
    public GalleryState(ICatService catService)
    {
        _catService = catService;
    }
    
    public async Task<Cat[]> InitializeAsync(int count)
    {
        var cats = new Cat[count];
        
        var indices = Enumerable.Range(0, count).ToArray();
        
        await Parallel.ForEachAsync(
            indices, 
            async (i,ct ) => 
            {
                cats[i] = await AddNewCatAsync();
            });

        return cats;
    }

    public async Task<Cat> AddNewCatAsync()
    {
        var cat = await _catService.GetRandomCatAsync();
        
        return cat;
    }
}