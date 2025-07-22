namespace CatGallery2.Application.Services.Entities;

public sealed class CatImage
{
    public long Id {get; set;}
    public string? ForeignId {get; set;}
    public DateTime? UploadDate {get; set;}
    public string? FileName {get; set;}
}