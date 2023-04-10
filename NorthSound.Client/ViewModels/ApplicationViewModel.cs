using NorthSound.Client.ViewModels.Base;
using NorthSound.Infrastructure.Services.Import.Base;
using System.Collections.Specialized;

namespace NorthSound.Client.ViewModels;

internal sealed class ApplicationViewModel : ViewModelBase
{
    public ApplicationViewModel Current { get; }

    public ApplicationViewModel(
        PlayerViewModel songVm,
        LibraryCollectionViewModel libraryVm,
        OnlineLibraryViewModel onlineLibraryVm,
        IImportService importer)
    {
        Current = this;

        PlayerVm = songVm;
        LibraryVm = libraryVm;
        OnlineLibraryVm = onlineLibraryVm;

        LocalImporter = importer;
        LocalImporter.ImportedCollection.CollectionChanged += OnImportCollectionChanged;
        LocalImporter.InitializeImportedStorage();
    }

    public IImportService LocalImporter { get; }

    public PlayerViewModel PlayerVm { get; }
    public LibraryCollectionViewModel LibraryVm { get; }
    public OnlineLibraryViewModel OnlineLibraryVm { get; }

    private void OnImportCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        LibraryVm.UpdateSongCollection(LocalImporter.ImportedCollection);
    }
}