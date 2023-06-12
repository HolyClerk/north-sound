using System.Text.Json.Serialization;

namespace NorthSound.Domain.Chat;

public class User
{
    [JsonPropertyName("UserName")]
    public string Name { get; set; }
}
