using NorthSound.Domain.Models;
using System;
using System.Collections.Generic;

namespace NorthSound.Infrastructure.Services.Base;

public interface IRepository<T>
{
    void Add(T item);
    void AddRange(IEnumerable<T> items);
    IEnumerable<T> GetStorageCollection();
    void Remove(T item);
    void Clear();
    void ReplaceCollection(IEnumerable<Song> items);
}
