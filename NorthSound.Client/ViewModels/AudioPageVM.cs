using NorthSound.Client.Infrastructure.Commands.Base;
using NorthSound.Client.Services;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using NorthSound.Domain.Models.Base;
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal class AudioPageVM : ViewModelBase
{
    private string _filterText = "";

    private ICollectionView? _playlistCollection;
    private ICollectionView? _audioPlaylists;

    SongViewModel _songViewModel;

    public AudioPageVM()
    {
        var soundsTest = new Song[]
        {
            new Song() { Name = "Закройте", Author = "Лампабикт", Path = new Uri(@"лампабикт - Закройте.mp3", UriKind.Relative) },
            new Song() { Name = "Ветивер", Author = "Wildways feat. polnalyubvi", Path = new Uri(@"Wildways feat. polnalyubvi - Ветивер (feat. polnalyubvi).mp3", UriKind.Relative) },
        };

        PlaylistCollection = CollectionViewSource.GetDefaultView(soundsTest);
        @SongViewModel = new SongViewModel();
    }

    public string FilterText
    {
        get => _filterText;
        set 
        {
            Set(ref _filterText, value);
            BindFilter();
        }
    }

    public ICollectionView? PlaylistCollection
    {
        get => _playlistCollection;
        set => Set(ref _playlistCollection, value);
    }

    public ICollectionView? AudioPlaylists
    {
        get => _playlistCollection;
        set => Set(ref _playlistCollection, value);
    }

    public SongViewModel @SongViewModel
    {
        get => _songViewModel;
        set => Set(ref _songViewModel, value);
    }

    private void BindFilter()
    {
        if (PlaylistCollection == null)
        {
            return;
        }

        PlaylistCollection.Filter = FilterAudio;
    }

    private bool FilterAudio(object obj)
    {
        var currentAudio = obj as Song;

        return string.IsNullOrWhiteSpace(FilterText)
            || currentAudio == null
            || currentAudio.IsAnyPropsContains(FilterText);
    }
}