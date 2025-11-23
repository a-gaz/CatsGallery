using CatGallery2.Application.Entities;
using CatGallery2.Application.Interfaces.Gateways;
using CatGallery2.Infrastructure.PostgresRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatGallery2.Auth;

public static class ServiceCollectionsExtensions
{
    public static async Task AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 1;
        }).AddEntityFrameworkStores<ApplicationDbContext>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        await CreateRolesAsync(serviceProvider);
        await CreateUsersAsync(serviceProvider);
    }

    private static async Task CreateRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
        string[] roleNames = ["USER", "ADMIN", "MODERATOR"];
    
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
    
    private static async Task CreateUsersAsync(ServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        var user = new ApplicationUser
        {
            UserName = "11"
        };
        var password = "11";
        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            var currentUser = await userManager.FindByNameAsync(user.UserName); 
                    
            var userresult = await userManager.AddToRoleAsync(currentUser, "ADMIN");
            if (!userresult.Succeeded)
            {
                throw new Exception("Error creating user");
            }
        }
    }
}