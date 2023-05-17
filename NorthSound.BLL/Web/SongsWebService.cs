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
    private readonly IServerInfo _serverInfo;
    private readonly HttpClient _httpClient;

    public SongsWebService(
        IRemoteSongRepository webRepository, 
        IServerInfo serverInfo)
    {
        _remoteRepository = webRepository;
        _serverInfo = serverInfo;
        _httpClient = new HttpClient();
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

    public async Task<ServerStatus> GetServerStatusAsync()
    {
        try
        {
            await _httpClient.GetAsync(_serverInfo.GetBaseUrl());
            return ServerStatus.Online;
        }
        catch (Exception)
        {
            return ServerStatus.Offline;
        }
    }
}