using CatGallery2.Application;
using CatGallery2.Infrastructure.CatApi;
using CatGallery2.Infrastructure.MinioStorage;
using CatGallery2.Infrastructure.PostgresRepository;
using CatGallery2.Infrastructure.RedisCache;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddMinio(builder.Configuration);
builder.Services.AddCatApi(builder.Configuration);
builder.Services.AddPostgresRepository(builder.Configuration);
builder.Services.AddRedisCache(builder.Configuration);
builder.Services.AddApp(builder.Configuration);


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

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