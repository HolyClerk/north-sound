using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.AudioPlayer.Base;
using System;
using System.Windows.Media;

namespace NorthSound.Infrastructure.Services.AudioPlayer;

public class AudioPlayer : IPlayer
{
    private readonly MediaPlayer _mediaPlayer;

    public AudioPlayer()
	{
        _mediaPlayer = new MediaPlayer();

        _mediaPlayer.MediaOpened += (o, e) 
            => IsPlaying = true;

        _mediaPlayer.MediaEnded += (o, e) =>
        {
            Ended?.Invoke();
            IsPlaying = false;
        };
    }

    public event Action Ended;

    public bool IsPlaying { get; private set; }

    public SongModel Current { get; private set; }

    public void Open(SongFile? song)
    {
        if (song is null)
            return;

        Current = song;
        _mediaPlayer.Close();
        _mediaPlayer.Open(song.Path);
    }

    public void OpenStream(VirtualSong? selectedSong)
    {
       // throw new NotImplementedException();
    }

    public void Play()
    {
        if (Current is VirtualSong)
        {
            return;
        }

        _mediaPlayer.Play();
        IsPlaying = true;
    }

    public void Pause()
    {
        _mediaPlayer.Pause();
        IsPlaying = false;
    }
}
