using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NorthSound.Infrastructure.Services;

public class SongStorageService : ICollectionObserver<Song>, IObservableStorage<Song>
{
    public SongStorageService()
    {
        Collection = new();
    }

    public ObservableCollection<Song> Collection
    { 
        get; 
        private set; 
    }

    public void ChangeObservableCollection(IEnumerable<Song> newObservableCollection)
    {
        Collection.Clear();

        foreach (var item in newObservableCollection)
        {
            Collection.Add(item);
        }
    }
}
