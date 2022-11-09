using NorthSound.Client.Infrastructure.Commands.Base;
using NorthSound.Client.Services;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using NorthSound.Domain.Models.Base;
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal class AudioPlayerViewModel : ViewModelBase
{
    private string _filterText = "";

    private ICollectionView? _playlistCollection;

    private Song? _selectedSong;

    public AudioPlayerViewModel()
    {
        var soundsTest = new Song[]
        {
            new Song() { Name = "Закройте", Author = "Лампабикт", Path = new Uri(@"лампабикт - Закройте.mp3", UriKind.Relative) },
            new Song() { Name = "Ветивер", Author = "Wildways feat. polnalyubvi", Path = new Uri(@"Wildways feat. polnalyubvi - Ветивер (feat. polnalyubvi).mp3", UriKind.Relative) },
        };

        PlaylistCollection = CollectionViewSource.GetDefaultView(soundsTest);
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

    public Song? SelectedSong
    {
        get => _selectedSong;
        set => Set(ref _selectedSong, value);
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