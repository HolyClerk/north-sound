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
        _mediaPlayer.MediaOpened += (s, e) => SongStarted?.Invoke(s, e);
        _mediaPlayer.MediaEnded += (s, e) => IsPlaying = false;
        _mediaPlayer.MediaFailed += (s, e) => IsPlaying = false;
    }

    public EventHandler SongStarted;

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

    public double GetDuration()
    {
        return _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
    }
    
    public void SetPosition(double seconds)
    {
        _mediaPlayer.Position = TimeSpan.FromSeconds(seconds);
    }

    public double GetPosition()
    {
        return _mediaPlayer.Position.TotalSeconds;
    }

    public void SetVolume(double value)
    {
        _mediaPlayer.Volume = value;
    }

    public double GetVolume()
    {
        return _mediaPlayer.Volume;
    }
}