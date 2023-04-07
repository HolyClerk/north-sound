using NorthSound.Domain.Models;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Infrastructure.Services.Base;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.AudioPlayer.Base;
using System;
using System.Collections.Specialized;

namespace NorthSound.Client.ViewModels;

internal sealed class PlayerViewModel : ViewModelBase
{
    private ObservableCollection<Song> _globalCollection;
    private Song? _selectedSong;

    private readonly IObservableStorage<Song> _observableStorage;
    private readonly IPlayer _player;

    public PlayerViewModel(
        IObservableStorage<Song> storage,
        IPlayer player) : base()
    {
        _globalCollection = new ObservableCollection<Song>();

        _observableStorage = storage;
        _observableStorage.Collection.CollectionChanged += OnObservableCollectionChanged;

        _player = player;
        _player.Ended += OnSongEnd;
    }

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

    // PlayerCollection - коллекция, выбранная в текущий момент времени
    public ObservableCollection<Song> PlayerCollection 
    {
        get => _globalCollection;
        set => Set(ref _globalCollection, value);
    }

    private void OnObservableCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        PlayerCollection = _observableStorage.Collection;
    }

    private void OnSongEnd()
    {
        var next = GetNextSong();

        if (next is null)
            return;

        SelectedSong = next;
    }

    private Song? GetNextSong()
    {
        try
        {
            var nextIndex = PlayerCollection.IndexOf(_selectedSong!) + 1;
            return PlayerCollection[nextIndex];
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
            var prevIndex = PlayerCollection.IndexOf(_selectedSong!) - 1;
            return PlayerCollection[prevIndex];
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
