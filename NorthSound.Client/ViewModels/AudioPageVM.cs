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

    private ICollectionView? _selectedPlaylist;

    private SongViewModel _songViewModel;
    private PlaylistViewModel _playlistViewModel;

    private Song _selectedSongBuffer;

    public AudioPageVM()
    {
        @SongViewModel = new SongViewModel();
        @PlaylistViewModel = new PlaylistViewModel();
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

    public ICollectionView? SelectedPlaylistCollection
    {
        get => _selectedPlaylist;
        set => Set(ref _selectedPlaylist, value);
    }

    // Это свойство служит интерфейсом-сеттером между выбранным плейлистом из
    // коллекции(типа Playlist из PlaylistViewModel),
    // и отображаемой коллекцией SelectedPlaylistCollection.
    public Playlist? SelectedPlaylist
    {
        set
        {
            SelectedPlaylistCollection = CollectionViewSource.GetDefaultView(value?.SongsCollection);
        }
    }

    public SongViewModel @SongViewModel
    {
        get => _songViewModel;
        set => Set(ref _songViewModel, value);
    }

    public PlaylistViewModel @PlaylistViewModel
    {
        get => _playlistViewModel;
        set => Set(ref _playlistViewModel, value);
    }

    private void BindFilter()
    {
        if (SelectedPlaylistCollection == null)
        {
            return;
        }

        SelectedPlaylistCollection.Filter = FilterAudio;
    }

    private bool FilterAudio(object obj)
    {
        var currentAudio = obj as Song;

        return string.IsNullOrWhiteSpace(FilterText)
            || currentAudio == null
            || currentAudio.IsAnyPropsContains(FilterText);
    }
}