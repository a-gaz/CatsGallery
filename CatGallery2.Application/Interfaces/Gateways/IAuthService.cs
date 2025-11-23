using CatGallery2.Application.Entities;

namespace CatGallery2.Application.Interfaces.Gateways;

public interface IAuthService
{
    Task<bool> LoginAsync(string userName, string password, CancellationToken cancellationToken);
    Task<bool> RegisterAsync(string userName, string password, CancellationToken cancellationToken);
    Task SignOutAsync(CancellationToken cancellationToken);
    
    Task<ApplicationUser> GetUserAdminAsync(CancellationToken cancellationToken);
    Task<ApplicationUser> GetCurrentUserAsync(string username, CancellationToken cancellationToken);
}