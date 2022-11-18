using NorthSound.Domain.Models.Base;

namespace NorthSound.Domain.Models;

public class Song : AudioFile
{
    public string? Description
    {
        get;
        set;
    }

    public bool IsAnyPropsContains(string filter)
    {
        return
            Name.ToLower().Contains(filter.ToLower()) ||
            Author.ToLower().Contains(filter.ToLower());
    }

    public override string ToString()
    {
        return $"{Name} - {Author}";
    }
}
