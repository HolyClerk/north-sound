namespace NorthSound.Domain.Models;

public class LocalSong : SongModel
{
    public Uri? Path { get; set; } = default!;

    public bool IsAnyPropsContains(string filter)
    {
        return
            Name.ToLower().Contains(filter.ToLower()) ||
            Author.ToLower().Contains(filter.ToLower());
    }
}
