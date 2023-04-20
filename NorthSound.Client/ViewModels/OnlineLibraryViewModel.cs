using NorthSound.BLL.Commands.Base;
using NorthSound.BLL.Facades.Base;
using NorthSound.BLL.Services.Storage.Base;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain;
using NorthSound.Domain.Models;
using System;
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

    private bool _isEnabled;
    public bool IsEnabled
    {
        get => _isEnabled;
        set => Set(ref _isEnabled, value);
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
        try
        {
            IsEnabled = true;
            var collection = await _webService.GetOnlineCollectionAsync();
            VirtualCollectionView = CollectionViewSource.GetDefaultView(collection);
            Filter = string.Empty;
            VirtualCollectionView.Refresh();
        }
        catch (Exception)
        {
            IsEnabled = false;
        }
    }

    private async Task DownloadSongAsync(VirtualSong? virtualSong)
    {
        try
        {
            if (virtualSong is null)
                return;

            var songFile = await _webService.DownloadAsync(virtualSong);

            if (songFile is null)
                return;

            _importService.Import(songFile);
        }
        catch (Exception)
        {
            // TODO: Сообщение об ошибке
        }
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