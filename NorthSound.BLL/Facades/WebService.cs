using NorthSound.Domain.Models;
using NorthSound.DAL.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NorthSound.BLL.Facades.Base;
using System.Net.Http;
using System.Printing;
using NorthSound.Domain;

namespace NorthSound.BLL.Facades;

public class WebService : IWebService
{
    private readonly IWebRepository _webRepository;
    private readonly HttpClient _httpClient;

    private const string ConnectionString = "http://localhost:5000/";
    private const string ApiBasePath = "api/library/";

    public WebService(IWebRepository webRepository)
    {
        _webRepository = webRepository;
        _httpClient = new HttpClient();
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

    public async Task<ServerStatus> GetServerStatusAsync()
    {
        try
        {
            await _httpClient.GetAsync(ConnectionString);
            return ServerStatus.Online;
        }
        catch (Exception)
        {
            return ServerStatus.Offline;
        }
    }
}