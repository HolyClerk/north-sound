using NorthSound.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NorthSound.BLL.Other;
using System.Net.Http;
using NorthSound.Domain;
using NorthSound.BLL.Facades.Base;

namespace NorthSound.BLL.Common.Import;

public class SongsWebService : ISongsWebService
{
    private readonly IRemoteSongRepository _remoteRepository;

    public SongsWebService(IRemoteSongRepository webRepository)
    {
        _remoteRepository = webRepository;
    }

    public async Task<IEnumerable<VirtualSong>> GetOnlineCollectionAsync()
    {
        var collection = await _remoteRepository.GetVirtualCollection();
        return collection;
    }

    public async Task<SongFile?> DownloadAsync(VirtualSong virtualSong)
    {
        if (virtualSong is null)
            return null;

        var songFile = await _remoteRepository.GetSongFileByEntity(virtualSong);
        return songFile;
    }
}