using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using NorthSound.BLL.Services.Storage.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.Specialized;
using NorthSound.BLL.Commands.Base;
using NorthSound.BLL.Common.Import.Base;

namespace NorthSound.Client.ViewModels;

internal sealed class LibraryCollectionViewModel : ViewModelBase
{
    private readonly ICollectionObserver<SongModel> _storageObserver;
    private readonly IImportService _importService;

    public LibraryCollectionViewModel(ICollectionObserver<SongModel> storageObserver, IImportService importService)
    {
        _storageObserver = storageObserver;
        _importService = importService;

        SongsCollectionView = CollectionViewSource.GetDefaultView(new ObservableCollection<SongFile>());
        SongsCollectionView.Filter = FilterCollection;

        _importService.ImportedCollection.CollectionChanged += OnImportCollectionChanged;
        _importService.InitializeImportedStorage();
    }

    private ICollectionView _songsCollectionView = default!;
    public ICollectionView SongsCollectionView
    {
        get => _songsCollectionView;
        set
        {
            if (_songsCollectionView == value)
                return;

            Set(ref _songsCollectionView, value);
            NotifyStorageObserver();
        }
    }

    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            Set(ref _filter, value);

            SongsCollectionView.Filter = FilterCollection;
        }
    }

    private bool FilterCollection(object entity)
    {
        if (entity is not SongFile song || string.IsNullOrWhiteSpace(Filter))
            return true;

        return song.IsAnyPropsContains(Filter);
    }

    private void NotifyStorageObserver()
    {
        _storageObserver.UpdateObservableCollection(
            CollectionType.Local, 
            (IEnumerable<SongFile>)SongsCollectionView.SourceCollection);
    }

    private void OnImportCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        SongsCollectionView = CollectionViewSource.GetDefaultView(_importService.ImportedCollection);
    }

    public RelayCommand ImportNewSongCommand
    {
        get => new RelayCommand((empty) => _importService.ExecuteImportDialogue());
    }
}
