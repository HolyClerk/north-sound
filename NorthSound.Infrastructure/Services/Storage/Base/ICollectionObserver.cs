using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorthSound.Infrastructure.Services.Storage.Base;

public interface ICollectionObserver<T>
{
    ObservableCollection<T> Collection { get; }

    void UpdateObservableCollection(CollectionType collectionType, IEnumerable<T> newObservableCollection);
    void SwitchObservableCollection(CollectionType collectionType);
}

public enum CollectionType
{
    Local,
    Virtual
}
