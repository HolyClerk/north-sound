namespace NorthSound.Domain.Models.Base;

public class AudioFile
{
    public string Name
    {
        get;
        set;
    }

    public string Author
    {
        get;
        set;
    }

    public Uri Path
    {
        get;
        set;
    }
}