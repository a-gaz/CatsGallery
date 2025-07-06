using CatsGallery.Application.Models;

namespace CatsGallery.Web.Services;

public interface IGalleryState
{
    List<Cat> Cats { get; }
    int CurrIndex { get; set; }
    Task InitializeAsync(int count);
    Task AddNewCatAsync();
}