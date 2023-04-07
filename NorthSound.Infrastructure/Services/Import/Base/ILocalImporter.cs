using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using System.Collections.ObjectModel;

namespace NorthSound.Infrastructure.Services.Import.Base;

public interface ILocalImporter
{
    AsyncRelayCommand AsyncImportCommand { get; }
    ObservableCollection<Song> ImportedCollection { get; }

    void InitializeImportedStorage();
}