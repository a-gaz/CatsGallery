using System.Text.Json.Serialization;

namespace CatsGallery.Infrastructure.Entities;

public record CatResponse
{ 
    [JsonPropertyName("id")] 
    public string Id { get; }
    
    [JsonPropertyName("tags")] 
    public IReadOnlyList<string> Tags { get; }
}