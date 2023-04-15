using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.AudioPlayer.Base;
using NorthSound.Infrastructure.Services.Static;
using System;
using System.Windows.Media;

namespace NorthSound.Infrastructure.Services.AudioPlayer;

public class AudioPlayer : IPlayer
{
    private readonly MediaPlayer _mediaPlayer;

    public AudioPlayer()
	{
        _mediaPlayer = new MediaPlayer();

        _mediaPlayer.MediaOpened += (o, e) =>
        {
            PlayerStateChanged?.Invoke(true);
        };

        _mediaPlayer.MediaEnded += (o, e) =>
        {
            PlayerStateChanged?.Invoke(false);
            SongEnded?.Invoke();
        };
    }

    public event Action<bool>? PlayerStateChanged;
    public event Action? SongEnded;

    public SongModel Current { get; private set; }

    public void Open(SongModel? song)
    {
        if (song is null)
            return;

        if (song is VirtualSong virtualSong)
            OpenVirtual(virtualSong);

        if (song is SongFile songFile)
            OpenFileStream(songFile);
    }

    private void OpenFileStream(SongFile songFile)
    {
        Current = songFile;

        _mediaPlayer.Close();
        _mediaPlayer.Open(songFile.Path);
    }

    private void OpenVirtual(VirtualSong? selectedSong)
    {
        if (selectedSong is null)
            return;

        var streamSource = VirtualSongConverter.GetUriLinkToStream(selectedSong);

        Current = selectedSong;
        
        _mediaPlayer.Close();
        _mediaPlayer.Open(streamSource);
    }

    public void Play()
    {
        if (Current is null)
            return;

        _mediaPlayer.Play();
        PlayerStateChanged?.Invoke(true);
    }

    public void Pause()
    {
        _mediaPlayer.Pause();
        PlayerStateChanged?.Invoke(false);
    }
}
