using NorthSound.Infrastructure.Services;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;

namespace NorthSound.Client.ViewModels.AudioVMs;

internal class PlaylistViewModel : ViewModelBase
{
    private LocalParser _parserService;
    private Playlist[]? _playlistsCollection;

    public PlaylistViewModel()
    {
        _parserService = new LocalParser(@"C:\Users\Public\Music\NorthSound");
        PlaylistsCollection = _parserService.GetLocalPlaylists();
    }

    public Playlist[]? PlaylistsCollection
    {
        get => _playlistsCollection;
        set => Set(ref _playlistsCollection, value);
    }
}