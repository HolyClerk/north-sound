using NorthSound.BLL.Other;
using NorthSound.Domain.Models;
using System.Net.Http.Json;

namespace NorthSound.Infrastructure;

public class RemoteSongRepository : IRemoteSongRepository
{
    private readonly HttpClient _httpClient;
    private readonly MediaReader _mediaReader;
    private readonly ServerInfo _serverInfo;

    public RemoteSongRepository()
    {
        _httpClient = new HttpClient();
        _mediaReader = new MediaReader();
        _serverInfo = new ServerInfo();
    }

    public async Task<SongFile?> GetSongFileByEntity(VirtualSong song)
    {
        var path = _mediaReader.DownloadPath + $"{song.Author} - {song.Name}.mp3";

        var response = await _httpClient.GetAsync(_serverInfo.GetCurrentSongUrl(song.Id));

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

    public async Task<IEnumerable<VirtualSong>> GetVirtualCollection()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<VirtualSong>>(_serverInfo.GetLibraryUrl());

        if (response is null)
            throw new Exception("Невозможно десериализовать JSON");

        return response;
    }

    public async Task<IEnumerable<VirtualSong>> GetVirtualCollectionByPattern(string pattern)
    {
        throw new NotImplementedException();
    }

    public async Task<Stream?> GetSongStreamByEntity(VirtualSong song)
    {
        var response = await _httpClient.GetAsync(_serverInfo.GetCurrentSongUrl(song.Id));
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
