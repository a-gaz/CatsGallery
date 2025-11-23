using CatGallery2.Web.ViewModelBuilders.GalleryStrategies;
using CatGallery2.Web.ViewModelBuilders.ModelBuildersAbstract;
using CatGallery2.Web.ViewModelBuilders.ModelBuildersAbstract.ModelBuilders;
using CatGallery2.Web.ViewModels;

namespace CatGallery2.Web.ViewModelBuilders;

public static class ServiceCollectionsExtensions
{
    public static void AddViewModelBuilder(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IViewModelBuilder<GalleryViewModel, IGalleryStrategy>, GalleryViewModelBuilder>();
        services.AddScoped<IViewModelBuilder<WishlistViewModel, IWishlistStrategy>, WishlistViewModelBuilder>();
    }
}