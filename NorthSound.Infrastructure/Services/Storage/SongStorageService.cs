using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.Storage.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NorthSound.Infrastructure.Services.Storage;

/// <summary>
/// Класс представляет собой текущую коллекцию, избранную на воспроизведение.
/// </summary>
public class SongStorageService : ICollectionObserver<SongModel>, IObservableStorage<SongModel>
{
    private IEnumerable<SongFile> _localCollection;
    private IEnumerable<VirtualSong> _virtualCollection;

    public SongStorageService()
    {
        Collection = new();

        _localCollection = new List<SongFile>();
        _virtualCollection = new List<VirtualSong>();
    }

    public ObservableCollection<SongModel> Collection
    {
        get;
        private set;
    }

    public void UpdateObservableCollection(CollectionType collectionType, IEnumerable<SongModel> newObservableCollection)
    {
        if (newObservableCollection is IEnumerable<SongFile> filesCollection)
            _localCollection = filesCollection;

        if (newObservableCollection is IEnumerable<VirtualSong> virtualCollection)
            _virtualCollection = virtualCollection;
    }

    public void SwitchObservableCollection(CollectionType collectionType)
    {
        IEnumerable<SongModel> collection = collectionType switch
        {
            CollectionType.Local => _localCollection,
            CollectionType.Virtual => _virtualCollection,
            _ => throw new Exception("Передан неверный параметр CollectionType"),
        };

        Collection.Clear();

        foreach (var item in collection)
        {
            Collection.Add(item);
        }
    }
}
