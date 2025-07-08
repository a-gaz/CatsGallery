using System.Text.Json.Serialization;

namespace CatsGallery.Application.Models;

public record CatResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("tags")]
    public IEnumerable<string> Tags { get; set; }
}