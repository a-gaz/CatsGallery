using CatGallery2.Application.Entities;
using CatGallery2.Web.ViewModels;

namespace CatGallery2.Web.EntityBuilderMiddlewares;

public interface ICatProductBuilderWebMiddleware
{
    Task CreateCatProductAsync(CatProductCreateViewModel model, ApplicationUser? currentUser,
        CancellationToken cancellationToken);

}