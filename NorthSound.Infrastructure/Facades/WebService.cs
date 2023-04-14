using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Web.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Facades;

public class WebService : IWebService
{
    private readonly IWebRepository _webRepository;

    public WebService(IWebRepository webRepository)
    {
        _webRepository = webRepository;
    }

    public Action<SongFile> Downloaded { get; set; }

    public async Task<IEnumerable<VirtualSong>> GetOnlineCollectionAsync()
    {
        var collection = await _webRepository.GetVirtualCollection();
        return collection;
    }

    public async Task<SongFile?> DownloadAsync(VirtualSong virtualSong)
    {
        if (virtualSong is null)
            return null;

        var songFile = await _webRepository.GetSongFileByEntity(virtualSong);
        return songFile;
    }

    public bool IsServerOnline() => _webRepository.IsServerOnline();
}