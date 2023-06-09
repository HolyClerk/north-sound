using NorthSound.Domain.POCO;

namespace NorthSound.Domain.Chat;

public class Result
{
    public ResponseStatus Status { get; set; }
    public string Details { get; set; }

    public static Result Ok()
    {
        return new Result
        {
            Status = ResponseStatus.Success,
            Details = string.Empty
        };
    }

    public static Result Failed(string details)
    {
        return new Result
        {
            Status = ResponseStatus.Failed,
            Details = details
        };
    }
}
