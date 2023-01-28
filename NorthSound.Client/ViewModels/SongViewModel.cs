using NorthSound.Domain.Models;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorthSound.Client.ViewModels;

internal class SongViewModel : ViewModelBase
{
    private readonly IObservableStorage<Song> _observableStorage;

    public SongViewModel(IObservableStorage<Song> storage, ISongImporter importer) : base()
    {
        Importer = importer;
        Importer.InitImport();

        _observableStorage = storage;
        _observableStorage.StorageChanged += UpdateCollection;

        _audioCollection = new ObservableCollection<Song>();
    }

    public ISongImporter Importer { get; }

    private Song? _selectedSong;
    public Song? SelectedSong
    {
        get => _selectedSong;
        set => Set(ref _selectedSong, value);
    }

    private ObservableCollection<Song> _audioCollection;
    public ObservableCollection<Song> AudioCollection 
    {
        get => _audioCollection;
        set => Set(ref _audioCollection, value);
    }

    private void UpdateCollection()
    {
        if (_observableStorage.GetStorageCollection() is not null)
        {
            AudioCollection = new(_observableStorage.GetStorageCollection());
        }
    }

    private RelayCommand _playCommand = null!;
    public RelayCommand PlayCommand
    {
        get
        {
            return _playCommand ??= new RelayCommand(execute =>
            {
                // TODO: Play Song
            }, canExecute => SelectedSong is not null);
        }
    }
}
