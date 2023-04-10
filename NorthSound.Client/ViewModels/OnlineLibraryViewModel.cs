using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Import;
using NorthSound.Infrastructure.Services.Web;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal sealed class OnlineLibraryViewModel : ViewModelBase
{
    private readonly IWebLibraryService _webLibrary;
    private readonly IWebStreamService _webStreamer;
    private readonly ILocator _locator;

    public OnlineLibraryViewModel(
        IWebLibraryService downloaderService,
        IWebStreamService streamService,
        ILocator locator)
    {
        _webLibrary = downloaderService;
        _webStreamer = streamService;
        _locator = locator;

        IntializeVirtualCollection();
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
        }
    }

    private VirtualSong? _selectedVirtualModel;
    public VirtualSong? SelectedVirtualModel
    {
        get => _selectedVirtualModel;
        set
        {
            if (_selectedVirtualModel == value)
                return;

            Set(ref _selectedVirtualModel, value);
        }
    }

    private AsyncRelayCommand _asyncDownloadCommand = null!;
    public AsyncRelayCommand AsyncDownloadCommand
    {
        get
        {
            return _asyncDownloadCommand ??= new AsyncRelayCommand(async Task () =>
            {
                if (_webStreamer.IsServerOnline() is false || _selectedVirtualModel is null)
                    return;

                var song = await _webStreamer.DownloadSong(_selectedVirtualModel);

                if (song is null)
                    return;

                _locator.Locate(song);
            });
        }
    }

    private async Task IntializeVirtualCollection()
    {
        /*if (_webLibrary.IsServerOnline() is false)
            return;*/

        var collection = await _webLibrary.FetchVirtualSongs();
        VirtualCollectionView = CollectionViewSource.GetDefaultView(collection);

    }
}