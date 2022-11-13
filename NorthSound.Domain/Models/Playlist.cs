namespace NorthSound.Domain.Models;

public class Playlist
{
	public Playlist()
	{
		SongsCollection = new List<Song>();
	}

    public List<Song> SongsCollection 
	{ 
		get; 
		set; 
	}

	public string Title
	{
		get;
		set;
	}

	public string Subtitle
	{
		get;
		set;
	}
}

