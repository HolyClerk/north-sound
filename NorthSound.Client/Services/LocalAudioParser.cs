using NorthSound.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NorthSound.Client.Services;

internal static class LocalAudioParser
{
    static readonly string s_playlistsPath;

    static LocalAudioParser()
    {
        s_playlistsPath = @"C:\Users\Public\Music\NorthSound";
        InitFolders(s_playlistsPath + @"\MainPlaylist");
    }

    public static Playlist[] GetLocalPlaylists()
    {
        var buffer = TryFindPlaylists();
        return buffer.ToArray();
    }

    private static List<Playlist> TryFindPlaylists()
    {
        var playlists = new List<Playlist>();
        string[] directories = Directory.GetDirectories(s_playlistsPath);

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
                    Song songTemp = MediaReader.ConvertTitle(songInfo);
                    playlist.SongsCollection.Add(songTemp);
                }
            }

            playlists.Add(playlist);
        }    

        return playlists;
    }

    private static void InitFolders(string playlistsPath)
    {
        var pathInfo = new DirectoryInfo(playlistsPath);

        if (pathInfo.Exists)
        {
            return;
        }

        pathInfo.Create();
    }
}