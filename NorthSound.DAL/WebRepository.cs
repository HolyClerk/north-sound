﻿using NorthSound.DAL.Base;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure;
using NorthSound.Infrastructure.Base;
using System.Net.Http.Json;

namespace NorthSound.DAL;

public class WebRepository : IWebRepository, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IServerInfo _serverInfo;

    public WebRepository(IServerInfo serverInfo)
    {
        _httpClient = new HttpClient();
        _serverInfo = serverInfo;
    }

    public async Task<SongFile?> GetSongFileByEntity(VirtualSong song)
    {
        var path = MediaReader.DownloadPath + $"{song.Author} - {song.Name}.mp3";
        var uri = _serverInfo.GetCurrentSongUrl(song.Id);
        var response = await _httpClient.GetAsync(uri);

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
        var uri = _serverInfo.GetLibraryUrl();
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<VirtualSong>>(uri);

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
        var uri = _serverInfo.GetCurrentSongUrl(song.Id);
        var response = await _httpClient.GetAsync(uri);
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
