using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Gateways;
using Microsoft.AspNetCore.Identity;

namespace CatGallery2.Auth;

internal sealed class AuthService : IAuthService
{
    
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AuthService(UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<bool> LoginAsync(string userName, string password, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(password))
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName, 
                    userName, 
                    true,                       // todo RememberMe
                    lockoutOnFailure: false);             // Не блокировать при ошибках

                if (result.Succeeded)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public async Task<bool> RegisterAsync(string userName, string password, CancellationToken cancellationToken)
    {
        if(!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(password))
        {
            if (!await IsUsernameTakenAsync(userName))
            {
                var user = new ApplicationUser
                {
                    UserName = userName
                };

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var currentUser = await _userManager.FindByNameAsync(user.UserName); 
                    
                    var roleresult = await _userManager.AddToRoleAsync(currentUser, "USER");
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return true;
                }
            }
        }
        
        return false;
    }

    public async Task SignOutAsync(CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<ApplicationUser> GetUserAdminAsync(CancellationToken cancellationToken)
    {
        var adminUsers = new List<ApplicationUser>();

        if (await _roleManager.RoleExistsAsync("Admin"))
        {
            adminUsers = (await _userManager.GetUsersInRoleAsync("Admin")).ToList();
        }
        
        return adminUsers.FirstOrDefault() ?? throw new Exception("User not found");
    }

    public async Task<ApplicationUser> GetCurrentUserAsync(string username, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentNullException("username");
        
        var user = await _userManager.FindByNameAsync(username);
        
        return user ?? throw new ArgumentException("username");
    }

    private async Task<bool> IsUsernameTakenAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;

        return await _userManager.FindByNameAsync(username) != null;
    }
}