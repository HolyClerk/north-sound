using NorthSound.Domain.Models;
using NorthSound.BLL.Services.AudioPlayer.Base;
using System;
using System.Windows.Media;
using System.Windows;

namespace NorthSound.BLL.Services.AudioPlayer;

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

    public SongModel? Current { get; private set; }

    public void Open(SongModel song)
    {
        if (song is null)
            return;

        _mediaPlayer.Close();

        if (song is VirtualSong virtualSong)
            OpenVirtual(virtualSong);

        if (song is SongFile songFile)
            OpenFileStream(songFile);

        Current = song;
    }

    public void SetVolume(double volume)
    {
        if (volume < 0 || volume > 1)
            return;

        _mediaPlayer.Volume = volume;
    }

    private void OpenFileStream(SongFile songFile)
    {
        _mediaPlayer.Open(songFile.Path);
    }

    private void OpenVirtual(VirtualSong selectedSong)
    {
        var streamSource = GetUriLinkToStream(selectedSong);
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

    public static Uri GetUriLinkToStream(VirtualSong virtualSong)
        => new("http://localhost:5000/api/library/" + virtualSong.Id); // TODO: FIX
}
