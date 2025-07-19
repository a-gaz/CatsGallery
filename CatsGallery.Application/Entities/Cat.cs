namespace CatsGallery.Application.Entities;

public class Cat
{
    public string Id { get; set; }
    public IReadOnlyList<string> Tags { get; set; }
    public byte[] ImageBytes { get; set; }
}