namespace CatGallery2.Application.Entities;

public sealed class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<TagProduct>? TagProducts { get; set; } = new List<TagProduct>();
}