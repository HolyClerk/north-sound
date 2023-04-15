using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Import.Base;
using NorthSound.Infrastructure.Services.Storage.Base;
using NorthSound.Infrastructure.Services.Web.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal sealed class OnlineLibraryViewModel : ViewModelBase
{
    private readonly ICollectionObserver<SongModel> _storageObserver;
    private readonly IImportService _importService;
    private readonly IWebService _webService;

    public OnlineLibraryViewModel(
        IImportService importService,
        IWebService webService,
        ICollectionObserver<SongModel> storageObserver)
    {
        _importService = importService;
        _webService = webService;
        _storageObserver = storageObserver;

        VirtualCollectionView = CollectionViewSource.GetDefaultView(new ObservableCollection<VirtualSong>());

        UpdateOnlineCollection();
    }

    private ICollectionView _virtualCollectionView = default!;
    public ICollectionView VirtualCollectionView
    {
        get => _virtualCollectionView;
        set
        {
            if (_virtualCollectionView == value)
                return;

            Set(ref _virtualCollectionView, value);
            NotifyStorageObserver();
        }
    }

    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            Set(ref _filter, value);
            VirtualCollectionView.Filter = FilterCollection;
        }
    }

    public AsyncRelayCommand AsyncDownloadCommand
    {
        get => new(
            execute: async Task (obj) => await DownloadSongAsync(obj as VirtualSong), 
            canExecute: (obj) => true);
    }

    public AsyncRelayCommand AsyncUpdateCommand
    {
        get => new(async Task (obj) => await UpdateOnlineCollection());
    }

    private async Task UpdateOnlineCollection()
    {
        var collection = await _webService.GetOnlineCollectionAsync();
        VirtualCollectionView = CollectionViewSource.GetDefaultView(collection);
        Filter = string.Empty;
        VirtualCollectionView.Refresh();
    }

    private async Task DownloadSongAsync(VirtualSong? virtualSong)
    {
        if (virtualSong is null)
            return;

        var songFile = await _webService.DownloadAsync(virtualSong);

        if (songFile is null)
            return;

        _importService.Import(songFile);
    }

    private void NotifyStorageObserver()
    {
        _storageObserver.UpdateObservableCollection(CollectionType.Virtual, (IEnumerable<VirtualSong>)VirtualCollectionView.SourceCollection);
    }

    private bool FilterCollection(object entity)
    {
        if (entity is not VirtualSong song || string.IsNullOrWhiteSpace(Filter))
            return true;

        return song.IsAnyPropsContains(Filter);
    }

    public void UpdateVirtualCollection(ObservableCollection<VirtualSong> collection)
    {
        VirtualCollectionView = CollectionViewSource.GetDefaultView(collection);
    }
}