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
        ILocalImporter importer)
    {
        SongVm = songVm;
        LibraryVm = libraryVm;
        LocalImporter = importer;

        LocalImporter.ImportedCollection.CollectionChanged += OnImportCollectionChanged;
        LocalImporter.InitializeImportedStorage();

        Current = this;
    }

    public ILocalImporter LocalImporter { get; }

    public PlayerViewModel SongVm { get; }
    public LibraryCollectionViewModel LibraryVm { get; }

    private void OnImportCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        LibraryVm.UpdateSongCollection(LocalImporter.ImportedCollection);
    }
}