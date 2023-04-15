using NorthSound.Domain.Models;
using NorthSound.BLL.Commands.Base;
using System.Collections.ObjectModel;

namespace NorthSound.BLL.Services.Import.Base;

public interface IImportService
{
    AsyncRelayCommand AsyncImportCommand { get; }
    ObservableCollection<SongFile> ImportedCollection { get; }

    void InitializeImportedStorage();
    void Import(SongFile entity);
}