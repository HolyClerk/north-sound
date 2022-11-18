using NorthSound.Client.Services;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;

namespace NorthSound.Client.ViewModels;

internal class PlaylistViewModel : ViewModelBase
{
    private Playlist[]? _playlistsCollection;

    public PlaylistViewModel()
    {
#if DEBUG
        PlaylistsCollection = LocalAudioParser.GetTemplatePlaylists();
#endif
       PlaylistsCollection = LocalAudioParser.GetLocalPlaylists();
    }

    public Playlist[]? PlaylistsCollection
    {
        get => _playlistsCollection;
        set => Set(ref _playlistsCollection, value);
    }
}