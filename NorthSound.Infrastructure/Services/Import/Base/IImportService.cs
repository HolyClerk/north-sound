using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using System.Collections.ObjectModel;

namespace NorthSound.Infrastructure.Services.Import.Base;

public interface IImportService
{
    AsyncRelayCommand AsyncImportCommand { get; }
    ObservableCollection<SongFile> ImportedCollection { get; }

    void InitializeImportedStorage();
    void Import(SongFile entity);
}