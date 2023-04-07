using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Base;
using NorthSound.Infrastructure.Services.Import.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services.Import;

public class LocalImporter : ILocalImporter
{
    private IFileImportService _fileImport;

    public LocalImporter(
        IFileImportService importService)
    {
        _fileImport = importService;
        ImportedCollection = new ObservableCollection<Song>();
    }

    public ObservableCollection<Song> ImportedCollection
    {
        get; private set;
    }

    private AsyncRelayCommand _asyncImportCommand = null!;
    public AsyncRelayCommand AsyncImportCommand
    {
        get
        {
            return _asyncImportCommand ??= new AsyncRelayCommand(async Task () =>
            {
                var song = _fileImport.ExecuteImport();

                if (song is not null)
                    ImportedCollection.Add(song);
            });
        }
    }

    public void InitializeImportedStorage()
    {
        var imported = _fileImport.GetImportedCollection();

        foreach (var item in imported)
            ImportedCollection.Add(item);
    }
}
