namespace NorthSound.Domain.Models;

public class Playlist
{
    public Playlist()
    {
        Songs = new List<SongModel>();
    }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    public List<SongModel> Songs { get; set; }
}

