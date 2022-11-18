using NorthSound.Domain.Models;
using System.Windows.Media;

namespace NorthSound.Client.ViewModels.Base;

class PlayerVmBase : ViewModelBase
{
    protected double _volume;
    protected bool IsPlaying;
    protected MediaPlayer MediaPlayer;

    public PlayerVmBase()
    {
        MediaPlayer = new MediaPlayer();
        MediaPlayer.MediaEnded += (s, e) => IsPlaying = false;
        MediaPlayer.MediaFailed += (s, e) => IsPlaying = false;
        MediaPlayer.MediaOpened += (s, e) =>
        {
            SongVolume = MediaPlayer.Volume;
            PlaySong();
        };
    }

    public double SongVolume
    {
        get => _volume;
        set
        {
            MediaPlayer.Volume = value;
            Set(ref _volume, value);
        }
    }

    #region Play & Pause обёртки
    protected void PlaySong()
    {
        MediaPlayer.Play();
        IsPlaying = true;
    }

    protected void PlaySong(Song song)
    {
        MediaPlayer.Open(song.Path);
        MediaPlayer.Play();
        IsPlaying = true;
    }

    protected void PauseSong()
    {
        MediaPlayer.Pause();
        IsPlaying = false;
    }
    #endregion
}

