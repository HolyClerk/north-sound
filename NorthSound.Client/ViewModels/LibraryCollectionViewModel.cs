using Microsoft.VisualBasic;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal sealed class LibraryCollectionViewModel : ViewModelBase
{
    private readonly ICollectionObserver<Song> _storageObserver;

    public LibraryCollectionViewModel(ICollectionObserver<Song> storageObserver)
    {
        _storageObserver = storageObserver;

        SongsCollectionView = CollectionViewSource.GetDefaultView(new ObservableCollection<Song>());
        SongsCollectionView.Filter = FilterCollection;
        SongsCollectionView.CollectionChanged += (sender, args) => NotifyStorageObserver();
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
        }
    }

    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            Set(ref _filter, value);
            SongsCollectionView.Refresh();
        }
    }

    private void NotifyStorageObserver()
    {
        _storageObserver.ChangeObservableCollection((IEnumerable<Song>)SongsCollectionView.SourceCollection);
    }

    private bool FilterCollection(object entity)
    {
        if (entity is not Song song || string.IsNullOrWhiteSpace(Filter))
            return false;

        return song.IsAnyPropsContains(Filter);
    }

    public void UpdateSongCollection(ObservableCollection<Song> collection)
    {
        SongsCollectionView = CollectionViewSource.GetDefaultView(collection);
        NotifyStorageObserver();
    }
}
