namespace NorthSound.Domain.Models;

public class Playlist
{
	public Playlist()
	{
		Songs = new List<Song>();
	}

	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;

    public List<Song> Songs { get; set; }
}

