using System.Text.Json.Serialization;

namespace CatsGallery.Gateway.Models;

public record CatResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("tags")]
    public IReadOnlyList<string> Tags { get; set; }
}