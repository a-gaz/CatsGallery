namespace CatsGallery.Application.Models;

public class CatResponse
{
    public string id { get; set; }
    public IEnumerable<string> tags { get; set; }
}