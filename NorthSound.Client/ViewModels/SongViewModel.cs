using NorthSound.Domain.Models;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Infrastructure.Services.Base;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.AudioPlayer.Base;
using System;

namespace NorthSound.Client.ViewModels;

internal sealed class SongViewModel : ViewModelBase
{
    private ObservableCollection<Song> _globalCollection;
    private Song? _selectedSong;

    private readonly IObservableStorage<Song> _observableStorage;
    private readonly IPlayer _player;

    public SongViewModel(
        IObservableStorage<Song> storage,
        IPlayer player) : base()
    {
        _globalCollection = new ObservableCollection<Song>();

        _observableStorage = storage;
        _observableStorage.StorageChanged += UpdateCollection;
        
        _player = player;
        _player.Ended += OnSongEnd;
    }

    // Текущая песня (глобально)
    public Song? SelectedSong
    {
        get => _selectedSong;
        set
        {
            Set(ref _selectedSong, value);

            _player.Open(_selectedSong);
            _player.Play();
        }
    }

    // GlobalCollection - коллекция, выбранная в текущий момент времени
    public ObservableCollection<Song> GlobalCollection 
    {
        get => _globalCollection;
        set => Set(ref _globalCollection, value);
    }

    private void OnSongEnd()
    {
        var next = GetNextSong();

        if (next is null)
            return;

        SelectedSong = next;
    }

    private void UpdateCollection()
    {
        if (_observableStorage.GetStorageCollection() is IEnumerable<Song> collection)
            GlobalCollection = new(collection);
    }

    private Song? GetNextSong()
    {
        try
        {
            var nextIndex = GlobalCollection.IndexOf(_selectedSong!) + 1;
            return GlobalCollection[nextIndex];
        }
        catch (Exception)
        {
            return null;
        }
    }

    private Song? GetPreviousSong()
    {
        try
        {
            var prevIndex = GlobalCollection.IndexOf(_selectedSong!) - 1;
            return GlobalCollection[prevIndex];
        }
        catch (Exception)
        {
            return null;
        }
    }

    private RelayCommand _playCommand = null!;
    private RelayCommand _nextSongCommand = null!;
    private RelayCommand _previousSongCommand = null!;

    public RelayCommand PlayCommand
    {
        get
        {
            return _playCommand ??= new RelayCommand(obj =>
            {
                if (_player.IsPlaying)
                {
                    _player.Pause();
                    return;
                }

                if (_player.Current != _selectedSong)
                {
                    _player.Open(_selectedSong);
                    _player.Play();
                    return;
                }

                _player.Play();

            }, obj => _selectedSong is not null);
        }
    }

    public RelayCommand NextSongCommand
    {
        get
        {
            return _nextSongCommand ??= new RelayCommand(obj =>
            {
                var next = GetNextSong();
                SelectedSong = next;
            }, obj => GetNextSong() is not null);
        }
    }

    public RelayCommand PreviousSongCommand
    {
        get
        {
            return _previousSongCommand ??= new RelayCommand(obj =>
            {
                var previous = GetPreviousSong();
                SelectedSong = previous;
            }, obj => GetPreviousSong() is not null);
        }
    }
}
