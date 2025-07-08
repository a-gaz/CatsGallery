using CatsGallery.Application;
using CatsGallery.Application.Models;

namespace CatsGallery.Web.Services;

public class GalleryState : IGalleryState
{
    private readonly IServiceProvider _serviceProvider;
    
    public int CurrIndex { get; set; }
    
    public GalleryState(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<List<Cat>> InitializeAsync(int count)
    {
        var cats = new List<Cat>();
        
        while (cats.Count < count)
        {
            cats.Add(await AddNewCatAsync());
        }

        return cats;
    }

    public async Task<Cat> AddNewCatAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var catService = scope.ServiceProvider.GetRequiredService<ICatService>();
        
        var cat = await catService.GetRandomCatAsync();
        
        return cat;
    }
}