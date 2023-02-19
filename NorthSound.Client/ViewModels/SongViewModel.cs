using NorthSound.Domain.Models;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Base;
using NorthSound.Infrastructure.Services.Import.Base;
using NorthSound.Infrastructure.Services.AudioPlayer.Base;
using System;
using System.Collections.ObjectModel;

namespace NorthSound.Client.ViewModels;

internal class SongViewModel : ViewModelBase
{
    private readonly IObservableStorage<Song> _observableStorage;

    public SongViewModel(IObservableStorage<Song> storage, 
        ISongImporter importer, 
        IPlayer player) : base()
    {
        _audioCollection = new ObservableCollection<Song>();

        _observableStorage = storage;
        _observableStorage.StorageChanged += UpdateCollection;

        Importer = importer;
        Importer.InitializeImportedStorage();

        Player = player;
    }

    public ISongImporter Importer { get; }

    public IPlayer Player { get; }

    private Song? _selectedSong;
    public Song? SelectedSong
    {
        get => _selectedSong;
        set
        {
            Set(ref _selectedSong, value);
            Player.PlayCommand.Execute(_selectedSong);
        }
    }

    private ObservableCollection<Song> _audioCollection;
    public ObservableCollection<Song> AudioCollection 
    {
        get => _audioCollection;
        set => Set(ref _audioCollection, value);
    }

    private RelayCommand _nextSongCommand = null!;
    public RelayCommand NextSongCommand
    {
        get
        {
            return _nextSongCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    var nextIndex = _audioCollection.IndexOf(_selectedSong!) + 1;
                    SelectedSong = _audioCollection[nextIndex];
                }
                catch (Exception) {}
            }, obj => _selectedSong is not null);
        }
    }

    private RelayCommand _previousSongCommand = null!;
    public RelayCommand PreviousSongCommand
    {
        get
        {
            return _previousSongCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    var previousIndex = _audioCollection.IndexOf(_selectedSong!) - 1;
                    SelectedSong = _audioCollection[previousIndex];
                }
                catch (Exception) { }
            }, obj => _selectedSong is not null);
        }
    }

    private void UpdateCollection()
    {
        if (_observableStorage.GetStorageCollection() is not null)
        {
            AudioCollection = new(_observableStorage.GetStorageCollection());
        }
    }
}
