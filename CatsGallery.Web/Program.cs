using CatsGallery.Application;
using CatsGallery.Gateway;
using CatsGallery.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ICatApiService, CatApiService>();
builder.Services.AddScoped<ICatService, CatService>();

builder.Services.AddSingleton<IGalleryState, GalleryState>();


builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gallery}/{action=Index}/{id?}");

app.Run();