namespace NorthSound.Domain.Models;

public class SongFile : SongModel
{
    public Uri? Path { get; set; } = default!;
}
