using System.Text.Json.Serialization;

namespace NorthSound.Domain.Chat;

public class MessagePOCO
{
    [JsonPropertyName("ReceiverUsername")]
    public string Username { get; set; }

    [JsonPropertyName("Message")]
    public string Text { get; set; }
}
