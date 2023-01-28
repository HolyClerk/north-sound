using System;
using System.Collections.Generic;

namespace NorthSound.Infrastructure.Services.Base;

public interface IObservableStorage<T>
{
    Action? StorageChanged { get; set; }
    IEnumerable<T> GetStorageCollection();
}
