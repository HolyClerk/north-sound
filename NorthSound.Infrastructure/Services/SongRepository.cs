using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NorthSound.Infrastructure.Services;

public class SongRepository : IRepository<Song>, IObservableStorage<Song>
{
    private List<Song> _collection;

    public SongRepository()
    {
        _collection = new List<Song>();
    }

    public IEnumerable<Song> GetStorageCollection()
        => _collection;

    public Action? StorageChanged { get; set; }

    public void Add(Song item)
    {
        _collection.Add(item);
        StorageChanged?.Invoke();
    }

    public void AddRange(IEnumerable<Song> items)
    {
        _collection.AddRange(items);
        StorageChanged?.Invoke();
    }

    public void ReplaceCollection(IEnumerable<Song> items)
    {
        _collection = items.ToList();
        StorageChanged?.Invoke();
    }

    public void Remove(Song item)
    {
        _collection.Remove(item);
        StorageChanged?.Invoke();
    }

    public void Clear()
    {
        _collection.Clear();
        StorageChanged?.Invoke();    
    }
}
