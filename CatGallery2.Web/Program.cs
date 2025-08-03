using CatGallery2.Application;
using CatGallery2.Infrastructure.CatApi;
using CatGallery2.Infrastructure.Minio;
using CatGallery2.Infrastructure.PostgresStorage;
using CatGallery2.Infrastructure.RedisStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddCatApi(builder.Configuration);
builder.Services.AddPostgresStorage(builder.Configuration);
builder.Services.AddRedisStorage(builder.Configuration);
builder.Services.AddMinio(builder.Configuration);
builder.Services.AddApp(builder.Configuration);


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseHsts();

app.UseRouting();

app.UseAuthorization();
app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gallery}/{action=Index}/");

app.Run();