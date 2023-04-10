using System.Text.Json.Serialization;

namespace NorthSound.Domain.Models;

public abstract class SongModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; } = default;

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("author")]
    public string Author { get; set; } = default!;
}
