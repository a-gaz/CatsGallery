using CatsGallery.Application;
using CatsGallery.Application.Models;

namespace CatsGallery.Web.Services;

public class GalleryState : IGalleryState
{
    private readonly IServiceProvider _serviceProvider;
    
    
    public List<Cat> Cats { get; }
    public int CurrIndex { get; set; }
    
    public GalleryState(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        Cats = new List<Cat>();
    }
    
    public async Task InitializeAsync(int count)
    {
        if (Cats.Count >= count) return;
        
        while (Cats.Count < count)
        {
            await AddNewCatAsync();
        }
    }

    public async Task AddNewCatAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var catService = scope.ServiceProvider.GetRequiredService<ICatService>();
        
        try
        {
            var cat = await catService.GetRandomCatAsync();
            
            Cats.Add(cat);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

}