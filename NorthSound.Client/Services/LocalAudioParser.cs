using NorthSound.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NorthSound.Client.Services;

internal static class LocalAudioParser
{
    static readonly string s_playlistsPath;

    static LocalAudioParser()
    {
        s_playlistsPath = @"C:\Users\Public\Music\NorthSound";
        TryInitFolders(s_playlistsPath + @"\MainPlaylist");
    }

    // DELETE_LATER
    public static Playlist[] GetTemplatePlaylists()
    {
        var songs = new Song[]
        {
            new Song() { Name = "Закройте", Author = "Лампабикт", Path = new Uri($@"{s_playlistsPath}\MainPlaylist\лампабикт - Закройте.mp3", UriKind.Relative) },
            new Song() { Name = "Ветивер", Author = "Wildways feat. polnalyubvi", Path = new Uri($@"{s_playlistsPath}\MainPlaylist\Wildways feat. polnalyubvi - Ветивер (feat. polnalyubvi).mp3", UriKind.Relative) },
        };

        return new Playlist[]
        {
            new Playlist()
            {
                SongsCollection = songs.ToList(),
                Subtitle = "Mine",
                Title = "Моя музыка"
            },

            new Playlist()
            {
                SongsCollection = songs.ToList(),
                Subtitle = "Подборка",
                Title = "Рок плейлист"
            },
        };
    }

    public static Playlist[] GetLocalPlaylists()
    {
        var buffer = TryFindPlaylists().Result;
        return buffer.ToArray();
    }

    private static async Task<List<Playlist>> TryFindPlaylists()
    {
        var playlists = new List<Playlist>();
        var mediaReader = new MediaReader();
        string[] directories = Directory.GetDirectories(s_playlistsPath);

        foreach (var directory in directories)
        {
            var playlist = new Playlist();
            string[] audiofilesPath = Directory.GetFiles(directory, "*.mp3");

            foreach (var audiofile in audiofilesPath)
            {
                var songTemp = await mediaReader.ConvertMetadataAsync(audiofile);

                if (songTemp == null)
                {
                    songTemp = mediaReader.ConvertTitle(audiofile);
                }

                playlist.SongsCollection.Add(songTemp);
            }

            playlists.Add(playlist);
        }    

        return playlists;
    }

    private static void TryInitFolders(string playlistsPath)
    {
        var pathInfo = new DirectoryInfo(playlistsPath);

        if (pathInfo.Exists)
        {
            return;
        }

        pathInfo.Create();
    }
}