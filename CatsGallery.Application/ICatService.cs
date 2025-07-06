using CatsGallery.Application.Models;

namespace CatsGallery.Application;

public interface ICatService
{
    Task<Cat> GetRandomCatAsync();
}