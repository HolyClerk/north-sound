using NorthSound.Domain.Models;
using NorthSound.Client.ViewModels.Base;
using System.Collections.ObjectModel;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.AudioPlayer.Base;
using System;
using System.Collections.Specialized;
using NorthSound.Infrastructure.Services.Storage.Base;

namespace NorthSound.Client.ViewModels;

internal sealed class PlayerViewModel : ViewModelBase
{
    private bool _isPlaying;

    private ObservableCollection<SongModel> _playerCollection;
    private SongModel? _selectedSong;

    private readonly IObservableStorage<SongModel> _observableStorage;
    private readonly IPlayer _player;

    public PlayerViewModel(
        IObservableStorage<SongModel> storage,
        IPlayer player) : base()
    {
        _playerCollection = new ObservableCollection<SongModel>();

        _observableStorage = storage;
        _observableStorage.Collection.CollectionChanged += OnObservableCollectionChanged;

        _player = player;
        _player.SongEnded += OnSongEnd;
        _player.PlayerStateChanged += OnPlayerStateChanged;
    }

    public SongModel? SelectedSong
    {
        get => _selectedSong;
        set
        {
            Set(ref _selectedSong, value);

            if (_selectedSong is SongFile localSong)
                _player.Open(localSong);

            if (_selectedSong is VirtualSong virtualSong)
                _player.OpenVirtual(virtualSong);

            _player.Play();
        }
    }

    // PlayerCollection - коллекция, выбранная в текущий момент времени
    // на ее основе формируется порядок очереди воспроизведения.
    public ObservableCollection<SongModel> PlayerCollection 
    {
        get => _playerCollection;
        set => Set(ref _playerCollection, value);
    }

    public bool IsPlaying
    {
        get => _isPlaying;
        set => Set(ref _isPlaying, value);
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

    private void OnPlayerStateChanged(bool isPlaying) 
        => IsPlaying = isPlaying;

    #region Commands & Executes
    private SongModel? GetNextSong()
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

    private SongModel? GetPreviousSong()
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
                if (IsPlaying)
                {
                    _player.Pause();
                    return;
                }

                if (_player.Current != _selectedSong)
                    _player.Open(_selectedSong as SongFile);

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
    #endregion
}
