using NorthSound.Domain.Models;
using NorthSound.Client.ViewModels.Base;
using System.Collections.ObjectModel;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.AudioPlayer.Base;
using System;
using System.Collections.Specialized;
using NorthSound.Infrastructure.Services.Storage.Base;
using System.Windows.Media;

namespace NorthSound.Client.ViewModels;

internal sealed class PlayerViewModel : ViewModelBase
{
    private bool _isPlaying;

    private ObservableCollection<SongModel> _playerCollection;
    private SongModel? _selectedSong;

    private readonly ICollectionObserver<SongModel> _observableStorage;
    private readonly IPlayer _player;

    public PlayerViewModel(
        ICollectionObserver<SongModel> storage,
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

            if (value is not null)
            {
                Open(value);
                Play();
            }
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
    private void Play() => _player.Play();

    private void Open(SongModel song)
    {
        if (song is VirtualSong)
            _observableStorage.SwitchObservableCollection(CollectionType.Virtual);

        if (song is SongFile)
            _observableStorage.SwitchObservableCollection(CollectionType.Local);

        _player.Open(song);
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
                ExecutePlayCommand(obj);
            }, obj => obj is VirtualSong or SongFile);
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

    private void ExecutePlayCommand(object obj)
    {
        if (obj is not SongModel songModel)
            return;

        if (IsPlaying && songModel == _selectedSong)
        {
            _player.Pause();
            return;
        }

        if (songModel != _selectedSong)
        {
            Open(songModel);
            SelectedSong = songModel;
        }

        Play();
    }

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
    #endregion
}
