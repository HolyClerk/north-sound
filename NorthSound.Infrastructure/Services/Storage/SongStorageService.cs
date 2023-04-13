using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.Storage.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NorthSound.Infrastructure.Services.Storage;

public class SongStorageService : ICollectionObserver<SongModel>, IObservableStorage<SongModel>
{
    /// <summary>
    /// Класс представляет собой текущую коллекцию, избранную на воспроизведение.
    /// </summary>
    public SongStorageService()
    {
        Collection = new();
    }

    public ObservableCollection<SongModel> Collection
    {
        get;
        private set;
    }

    public void ChangeObservableCollection(IEnumerable<SongModel> newObservableCollection)
    {
        Collection.Clear();

        foreach (var item in newObservableCollection)
        {
            Collection.Add(item);
        }
    }
}
