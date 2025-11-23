namespace CatGallery2.Web.ViewModelBuilders.ModelBuildersAbstract;

public interface IViewModelBuilder<TViewModel, in TStrategy>
{
    IViewModelBuilder<TViewModel, TStrategy> WithStrategy(TStrategy strategy);
    Task<TViewModel> BuildAsync(string userName, CancellationToken cancellationToken);
}