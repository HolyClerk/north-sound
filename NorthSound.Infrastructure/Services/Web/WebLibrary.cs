using NorthSound.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace NorthSound.Infrastructure.Services.Web;

public class WebLibrary : IWebLibraryService, IWebStreamService, IDisposable
{
    private readonly HttpClient _httpClient;
    private const string ConnectionString = "http://localhost:5000";

    public WebLibrary()
    {
        _httpClient = new HttpClient();
    }

    public Task<LocalSong?> DownloadSong(VirtualSong song)
    {
        return null;

    }

    public async Task <IEnumerable<VirtualSong>> FetchVirtualSongs()
    {
        var response = await _httpClient.GetAsync(ConnectionString + "/api/library");

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<VirtualSong>>(json);

        if (result is null)
            MessageBox.Show("Error");

        return result;
        
    }

    public async Task <IEnumerable<VirtualSong>> FetchVirtualSongs(string pattern)
    {
        var response = await _httpClient.GetAsync(ConnectionString + "/api/library");

        using (Stream json = await response.Content.ReadAsStreamAsync())
        {
            var result = await JsonSerializer.DeserializeAsync<IEnumerable<VirtualSong>>(json);

            if (result is null)
                return new List<VirtualSong>();

            return result;
        }
    }

    public Task<Stream>? GetSongStream(VirtualSong song)
    {
        return null;
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
