using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NorthSound.Client.ViewModels;

internal class PlaylistViewModel : ViewModelBase
{
    private Playlist[]? _playlistsCollection;

    public PlaylistViewModel()
    {
        var songs = new List<Song>()
        {
            new Song() { Name = "Закройте", Author = "Лампабикт", Path = new Uri(@"лампабикт - Закройте.mp3", UriKind.Relative) },
            new Song() { Name = "Ветивер", Author = "Wildways feat. polnalyubvi", Path = new Uri(@"Wildways feat. polnalyubvi - Ветивер (feat. polnalyubvi).mp3", UriKind.Relative) },
        };

        PlaylistsCollection = new Playlist[]
        {
            new Playlist()
            {
                SongsCollection = songs,
                Subtitle = "Mine",
                Title = "Моя музыка"
            },

            new Playlist()
            {
                SongsCollection = songs,
                Subtitle = "Подборка",
                Title = "Рок плейлист"
            },
        };
    }

    public Playlist[]? PlaylistsCollection
    {
        get => _playlistsCollection;
        set => Set(ref _playlistsCollection, value);
    }
}