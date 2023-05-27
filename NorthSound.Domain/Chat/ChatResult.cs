using NorthSound.Domain.POCO;

namespace NorthSound.Domain.Chat;

public class ChatResult
{
    public ResponseStatus Status { get; set; }
    public string Details { get; set; }

    public static ChatResult Ok()
    {
        return new ChatResult
        {
            Status = ResponseStatus.Success,
            Details = string.Empty
        };
    }

    public static ChatResult Failed(string details)
    {
        return new ChatResult
        {
            Status = ResponseStatus.Failed,
            Details = details
        };
    }
}
