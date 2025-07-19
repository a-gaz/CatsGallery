using CatsGallery.Application.Interfaces;
using CatsGallery.Infrastructure.Services;
using CatsGallery.Shared.Configuration;
using CatsGallery.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// todo тут переделать все
var webProjectPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\"));
var sharedConfigPath = Path.Combine(
    webProjectPath,
    "CatsGallery.Shared");
builder.Configuration
    .SetBasePath(sharedConfigPath);
builder.Services.Configure<Settings>(
    builder.Configuration.GetSection(Settings.DefaultConfigFile));

builder.Services.AddHttpClient<ICatApiService, CatApiService>();
builder.Services.AddSingleton<ICatGalleryService, CatGalleryService>();
builder.Services.AddScoped<IBrowserSession, BrowserSession>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".CatsGallery.Session";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); 
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gallery}/{action=Index}/{id?}");

app.Run();