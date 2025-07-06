namespace CatsGallery.Application.Models;

public class Cat
{
    public int Id { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public byte[] ImageBytes { get; set; }
}