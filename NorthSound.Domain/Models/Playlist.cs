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

	public string PlaylistName
	{
		get;
		set;
	}

	public string PlaylistAuthor
	{
		get;
		set;
	}
}

