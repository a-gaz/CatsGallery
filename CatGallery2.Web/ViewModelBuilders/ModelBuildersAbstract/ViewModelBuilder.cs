using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace CatGallery2.Web.ViewModelBuilders.ModelBuildersAbstract;

public abstract class ViewModelBuilder<TViewModel, TStrategy> : IViewModelBuilder<TViewModel, TStrategy>
{
    protected readonly IGalleryService GalleryService;
    protected readonly IWishlistService _wishlistService;
    protected readonly UserManager<ApplicationUser> _userManager;
    protected readonly ILogger<ViewModelBuilder<TViewModel, TStrategy>> _logger;
    
    protected ApplicationUser? _currentUser;
    
    protected TStrategy? _strategy;
    
    public ViewModelBuilder(IGalleryService galleryService, 
        IWishlistService wishlistService, 
        UserManager<ApplicationUser> userManager,
        ILogger<ViewModelBuilder<TViewModel, TStrategy>> logger)
    {
        GalleryService = galleryService;
        _wishlistService = wishlistService;
        _logger = logger;
        _userManager = userManager;
    }
    
    public IViewModelBuilder<TViewModel, TStrategy> WithStrategy(TStrategy strategy)
    {
        _strategy = strategy;
        return this;
    }
    
    protected async Task Validate(string? userName)
    {
        if (_strategy == null)
            throw new InvalidOperationException("Стратегия не инициализирована");
        
        if(userName != null)
            _currentUser = await _userManager.FindByNameAsync(userName);
    }
    
    public abstract Task<TViewModel> BuildAsync(string userName, CancellationToken cancellationToken);
}