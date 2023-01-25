using NorthSound.Domain.Models;
using System.Collections.Generic;
using System.IO;

namespace NorthSound.Infrastructure.Services;

public class LocalParser
{
    private readonly string _playlistsPath;

    public LocalParser()
    {
        _playlistsPath = @"C:\Users\Public\Music\NorthSound";
        InitFolders(_playlistsPath);
    }

    public LocalParser(string playlistsPath)
    {
        _playlistsPath = playlistsPath;
        InitFolders(playlistsPath + @"\MainPlaylist");
    }

    public Playlist[]? GetLocalPlaylists()
    {
        _ = TryFindPlaylists(out var playlists);
        return playlists.ToArray();
    }

    private bool TryFindPlaylists(out List<Playlist> playlists)
    {
        playlists = new List<Playlist>();
        string[] directories = Directory.GetDirectories(_playlistsPath);

        foreach (var directory in directories)
        {
            var playlist = new Playlist()
            {
                Title = "Temp",
                Subtitle = "sub",
            };

            string[] audiofilesPath = Directory.GetFiles(directory, "*.mp3");

            foreach (var audiofile in audiofilesPath)
            {
                if (MediaReader.TryFindMediaFile(audiofile, out var songInfo))
                {
                    Song songTemp = MediaReader.ConvertToSong(songInfo);
                    playlist.SongsCollection.Add(songTemp);
                }
            }

            playlists.Add(playlist);
        }    

        return true;
    }

    private void InitFolders(string playlistsPath)
    {
        var pathInfo = new DirectoryInfo(playlistsPath);

        if (pathInfo.Exists)
        {
            return;
        }

        pathInfo.Create();
    }
}