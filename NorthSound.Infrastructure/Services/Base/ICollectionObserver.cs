using System.Collections.Generic;

namespace NorthSound.Infrastructure.Services.Base;

public interface ICollectionObserver<T>
{
    void ChangeObservableCollection(IEnumerable<T> newObservableCollection);
}
