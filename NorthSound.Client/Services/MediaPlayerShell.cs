using NorthSound.Domain.Models;
using System;
using System.Windows.Media;

namespace NorthSound.Client.Services;

internal class MediaPlayerShell
{
    private MediaPlayer _mediaPlayer;

    public MediaPlayerShell()
    {
        _mediaPlayer = new MediaPlayer();
        _mediaPlayer.MediaEnded += (s, e) => IsPlaying = false;
        _mediaPlayer.MediaFailed += (s, e) => IsPlaying = false;
        _mediaPlayer.MediaOpened += (s, e) => IsPlaying = false;
    }

    public bool IsPlaying 
    { 
        get; 
        set; 
    }

    public Song CurrentSong 
    {
        set => _mediaPlayer.Open(value.Path);
    }

    public void Play()
    {
        _mediaPlayer.Play();
        IsPlaying = true;
    }

    public void Pause()
    {
        _mediaPlayer.Pause();
        IsPlaying = false;
    }  

    public int GetDuration()
    {
        return _mediaPlayer.NaturalDuration.TimeSpan.Seconds;
    }

    public void SetVolume(double value)
    {
        _mediaPlayer.Volume = value;
    }
}