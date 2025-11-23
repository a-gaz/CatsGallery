namespace CatGallery2.Application.Interfaces.EntityBuilders;

public interface ICatProductBuilder
{
    Task LoadNewCatsAsync(CancellationToken cancellationToken);
}