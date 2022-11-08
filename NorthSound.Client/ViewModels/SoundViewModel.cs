using NorthSound.Client.Infrastructure.Commands;
using NorthSound.Client.Services;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal class SoundViewModel : ViewModelBase
{
    private string _filterText = "";
    private ICollectionView? _audioPlaylistItems;
    private PlayerShellService _playerShell;
    private RelayCommand? _playCommand;
    private Sound? _selectedAudio;

    public SoundViewModel()
    {
        _playerShell = new PlayerShellService();

        var soundsTest = new Sound[]
        {
            new Sound() { Name = "Закройте", Author = "Лампабикт", RelativePath = @"лампабикт - Закройте.mp3" },
            new Sound() { Name = "Ветивер", Author = "Wildways feat. polnalyubvi", RelativePath = @"Wildways feat. polnalyubvi - Ветивер (feat. polnalyubvi).mp3" },
        };

        AudioPlaylistItems = CollectionViewSource.GetDefaultView(soundsTest);
        BindFilter(FilterAudio);
    }

    public string FilterText
    {
        get => _filterText;
        set 
        {
            Set(ref _filterText, value);
            BindFilter(FilterAudio);
        }
    }

    public ICollectionView? AudioPlaylistItems
    {
        get => _audioPlaylistItems;
        set => Set(ref _audioPlaylistItems, value);
    }

    public Sound? SelectedAudio
    {
        get => _selectedAudio;
        set
        {
            Set(ref _selectedAudio, value);
            _playerShell.SelectedAudioSound = value;
        }
    }

    public RelayCommand PlayCommand
    {
        get
        {
            return _playCommand ?? (_playCommand = new RelayCommand(obj =>
            {
                _playerShell.Play();
            }));
        }
    }

    private void BindFilter(Predicate<object> filterAudio)
    {
        if (AudioPlaylistItems == null)
        {
            return;
        }

        AudioPlaylistItems.Filter = filterAudio;
    }

    private bool FilterAudio(object obj)
    {
        var currentAudio = obj as Sound;
        return string.IsNullOrWhiteSpace(FilterText)
            || currentAudio == null
            || currentAudio.IsAnyPropsContains(FilterText);
    }
}

