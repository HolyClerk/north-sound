using NorthSound.Domain.Models;
using NorthSound.DAL.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NorthSound.BLL.Facades.Base;

namespace NorthSound.BLL.Facades;

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