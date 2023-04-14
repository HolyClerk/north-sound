using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.Static;
using NorthSound.Infrastructure.Services.Web.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services.Web;

public class WebRepository : IWebRepository, IDisposable
{
    private readonly HttpClient _httpClient;

    private const string ConnectionString = "http://localhost:5000/";
    private const string ApiBasePath = "api/library/";

    public WebRepository()
    {
        _httpClient = new HttpClient();
    }

    public async Task<SongFile?> GetSongFileByEntity(VirtualSong song)
    {
        var path = MediaReader.DownloadPath + $"{song.Author} - {song.Name}.mp3";

        var response = await _httpClient.GetAsync(ConnectionString + ApiBasePath + song.Id);

        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            await stream.CopyToAsync(fileStream);
        }

        var songFile = new SongFile()
        {
            Author = song.Author,
            Name = song.Name,
            Path = new Uri(path)
        };

        return songFile;
    }

    public async Task <IEnumerable<VirtualSong>> GetVirtualCollection()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<VirtualSong>>(ConnectionString + ApiBasePath);

        if (response is null)
            throw new Exception("Невозможно десериализовать JSON");

        return response;
    }

    public async Task <IEnumerable<VirtualSong>> GetVirtualCollectionByPattern(string pattern)
    {
        throw new NotImplementedException();
    }

    public async Task<Stream?> GetSongStreamByEntity(VirtualSong song)
    {
        var response = await _httpClient.GetAsync(ConnectionString + ApiBasePath + song.Id);
        var stream = await response.Content.ReadAsStreamAsync();
        return stream;
    }

    public Task<bool> UpdateSong(SongFile song)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddSong(SongFile song)
    {
        throw new NotImplementedException();
    }

    public bool IsServerOnline()
    {
        return true;
    }

    private bool _isDisposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                // TODO: освободить управляемое состояние (управляемые объекты)
            }

            _httpClient?.Dispose();
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
