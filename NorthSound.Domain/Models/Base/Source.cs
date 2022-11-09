namespace NorthSound.Domain.Models.Base;

public class Source
{
    public Source(string relativePath)
    {
        RelativePath = relativePath;
    }

    public string? AbsolutePath
    {
        get;
        set;
    }

    public string RelativePath
    {
        get;
        set;
    }
}
