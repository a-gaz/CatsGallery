using CatsGallery.Web.Services;
using CatsGallery.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CatsGallery.Web.Controllers;

public class GalleryController : Controller
{
    private readonly IGalleryState _galleryState;
    private const int InitialCatCount = 3;
    public GalleryController(IGalleryState galleryState)
    {
        _galleryState = galleryState;
    }

    public async Task<IActionResult> Index()
    {
        await _galleryState.InitializeAsync(InitialCatCount);

        _galleryState.CurrIndex = 1;
        
        var model = BuildViewModel();
        
        return View(model);
    }

    public async Task<IActionResult> NextCat()
    {
        await _galleryState.AddNewCatAsync();
        // TODO Тут ты мутируешь стейт по какой-то логике снаружи стейта, в контроллере. Так делать нельзя
        _galleryState.CurrIndex = (_galleryState.CurrIndex + 1) % _galleryState.Cats.Count;
        
        var model = BuildViewModel();
        return PartialView("GalleryPartial", model);
    }
    
    public async Task<IActionResult> PrevCat()
    { 
        // TODO Тут ты мутируешь стейт по какой-то логике снаружи стейта, в контроллере. Так делать нельзя
        _galleryState.CurrIndex = (_galleryState.CurrIndex - 1 + _galleryState.Cats.Count) % _galleryState.Cats.Count;
        
        var model = BuildViewModel();
        return PartialView("GalleryPartial", model);
    }

    private GalleryViewModel BuildViewModel()
    {
        var cats = _galleryState.Cats;
        var currentIndex = _galleryState.CurrIndex;
        
        int prevIndex = (currentIndex - 1 + cats.Count) % cats.Count;
        int nextIndex = (currentIndex + 1) % cats.Count;
        // TODO Еще у тебя в карусели при тыке каждый раз все три картинки загружаются. Надо как-то иначе это делать. Мб сохранять их на диск и сделать обычные урлы. Надо тут подумать
        return new GalleryViewModel
        {
            PrevCat = cats[prevIndex],
            CurrCat = cats[currentIndex],
            NextCat = cats[nextIndex],
        };
    }
}