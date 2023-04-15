using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.Import.Base;
using NorthSound.Infrastructure.Services.Web.Base;
using System;
using System.Collections.Specialized;

namespace NorthSound.Client.ViewModels;

internal sealed class ApplicationViewModel : ViewModelBase
{
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

        ImportService = importer;
        ImportService.ImportedCollection.CollectionChanged += OnImportCollectionChanged;
        ImportService.InitializeImportedStorage();
    }

    public ApplicationViewModel Current { get; }

    public IImportService ImportService { get; }

    public PlayerViewModel PlayerVm { get; }
    public LibraryCollectionViewModel LibraryVm { get; }
    public OnlineLibraryViewModel OnlineLibraryVm { get; }

    private void OnImportCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        LibraryVm.UpdateSongCollection(ImportService.ImportedCollection);
    }
}