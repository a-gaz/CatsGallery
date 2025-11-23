using CatGallery2.Application.Interfaces.Gateways;
using CatGallery2.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CatGallery2.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<GalleryController> _logger;

    public AuthController(IAuthService authService, ILogger<GalleryController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Login(CancellationToken cancellationToken)
    {
         return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(AuthViewModel model, CancellationToken cancellationToken)
    {
        var res = await _authService.LoginAsync(model.UserName, model.Password, cancellationToken);
        if (res)
        {
            return RedirectToAction("Index", "Gallery");
        }
        else
        {
            ModelState.AddModelError("UserName", "Это имя пользователя уже занято");
            return View();
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Register(CancellationToken cancellationToken)
    {
         return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(AuthViewModel model, CancellationToken cancellationToken)
    {
        var res = await _authService.RegisterAsync(model.UserName, model.Password, cancellationToken);
        if (res)
        {
            return RedirectToAction("Index", "Gallery");
        }
        else
        {
            ModelState.AddModelError("UserName", "Это имя пользователя уже занято");
            return View();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await _authService.SignOutAsync(cancellationToken);
        return RedirectToAction("Index", "Gallery");
    }
}