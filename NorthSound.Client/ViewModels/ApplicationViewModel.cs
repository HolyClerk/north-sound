using NorthSound.Client.ViewModels.Base;
using NorthSound.Infrastructure.Services.Import.Base;
using System.Collections.Specialized;

namespace NorthSound.Client.ViewModels;

internal class ApplicationViewModel : ViewModelBase
{
    public ApplicationViewModel Current { get; }

    public ApplicationViewModel(
        SongViewModel songVm,
        LibraryViewModel libraryVm,
        ILocalImporter importer)
    {
        SongVm = songVm;
        LibraryVm = libraryVm;
        LocalImporter = importer;

        LocalImporter.ImportedCollection.CollectionChanged += OnLocalCollectionChanged;
        LocalImporter.InitializeImportedStorage();

        Current = this;
    }

    public ILocalImporter LocalImporter { get; }

    public SongViewModel SongVm { get; }
    public LibraryViewModel LibraryVm { get; }

    private void OnLocalCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        LibraryVm.LocalAudioCollection = LocalImporter.ImportedCollection;
    }
}