using CatsGallery.Application.Models;

namespace CatsGallery.Web.Services;

public interface IGalleryState
{
    Task<Cat[]> InitializeAsync(int count);
    Task<Cat> AddNewCatAsync();
}