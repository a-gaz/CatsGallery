using CatsGallery.Application.Models;

namespace CatsGallery.Web.Services;

public interface IGalleryState
{
    int CurrIndex { get; set; }
    Task<List<Cat>> InitializeAsync(int count);
    Task<Cat> AddNewCatAsync();
}