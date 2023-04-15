using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using NorthSound.BLL.Services.Storage.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal sealed class LibraryCollectionViewModel : ViewModelBase
{
    private readonly ICollectionObserver<SongModel> _storageObserver;

    public LibraryCollectionViewModel(ICollectionObserver<SongModel> storageObserver)
    {
        _storageObserver = storageObserver;

        SongsCollectionView = CollectionViewSource.GetDefaultView(new ObservableCollection<SongFile>());
        SongsCollectionView.Filter = FilterCollection;
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
        _storageObserver.UpdateObservableCollection(CollectionType.Local, (IEnumerable<SongFile>)SongsCollectionView.SourceCollection);
    }

    public void UpdateSongCollection(ObservableCollection<SongFile> collection)
    {
        SongsCollectionView = CollectionViewSource.GetDefaultView(collection);
    }
}
