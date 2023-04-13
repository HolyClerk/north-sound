using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Web.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Facades;

public class WebService : IWebService
{
    private readonly IWebRepository _webRepository;

    public WebService(IWebRepository webRepository)
    {
        _webRepository = webRepository;
        VirtualCollection = new();
    }

    public AsyncRelayCommand AsyncDownloadCommand
    {
        get => new(async Task (obj) => await DownloadSong(obj as VirtualSong));
    }

    public AsyncRelayCommand AsyncUpdateCommand
    {
        get => new(async Task (obj) => await InitializeOnlineCollection());
    }

    public ObservableCollection<VirtualSong> VirtualCollection { get; }

    public Action<SongFile> Downloaded { get; set; }

    public async Task InitializeOnlineCollection()
    {
        var collection = await _webRepository.GetVirtualCollection();
        VirtualCollection.Clear();

        foreach (var virtualSong in collection)
        {
            VirtualCollection.Add(virtualSong);
        }
    }

    private async Task DownloadSong(VirtualSong? virtualSong)
    {
        if (virtualSong is null)
            return;

        var songFile = await _webRepository.GetSongFileByEntity(virtualSong);

        if (songFile is null)
            return;

        Downloaded?.Invoke(songFile);
    }
}