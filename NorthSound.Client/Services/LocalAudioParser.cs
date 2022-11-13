using NorthSound.Domain.Models;
using NorthSound.Domain.Models.Base;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NorthSound.Client.Services;

internal static class LocalAudioParser
{
    static readonly string s_playlistsPath;

    static LocalAudioParser()
    {
        s_playlistsPath = @"C:\Users\Public\Music\NorthSound";
        TryInitFolders(s_playlistsPath + @"\MainPlaylist");
        GetFindedPlaylists();
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

    public static async void GetFindedPlaylists()
    {
        var mediaReader = new MediaReader();

        string[] directories = Directory.GetDirectories(s_playlistsPath);

        foreach (var directory in directories)
        {
            string[] audiofilesPath = Directory.GetFiles(directory, "*.mp3");

            Debug.WriteLine(directory + ": ");

            foreach (var audiofile in audiofilesPath)
            {
                var songTemp = await mediaReader.ConvertMetadataAsync(audiofile);

                if (songTemp == null)
                {
                    songTemp = mediaReader.ConvertTitle(audiofile);
                    Debug.WriteLine($"{songTemp.Name} - {songTemp.Author}");
                }
            }
        }    
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