using NorthSound.Domain.Models.Base;
using System.Windows.Media;

namespace NorthSound.Client.ViewModels.Base;

class PlayerVmBase : ViewModelBase
{
    protected bool IsPlaying;

    protected double _volume;
    protected double _speedRatio;

    protected MediaPlayer MediaPlayer;

    public PlayerVmBase()
    {
        MediaPlayer = new MediaPlayer();
        MediaPlayer.MediaEnded += (s, e) => IsPlaying = false;
        MediaPlayer.MediaFailed += (s, e) => IsPlaying = false;
        MediaPlayer.MediaOpened += (s, e) =>
        {
            AudioVolume = MediaPlayer.Volume;
            SpeedRatio = MediaPlayer.SpeedRatio;
            PlayAudio();
        };
    }

    public double AudioVolume
    {
        get => _volume;
        set
        {
            MediaPlayer.Volume = value;
            Set(ref _volume, value);
        }
    }

    public double SpeedRatio
    {
        get => _speedRatio;
        set
        {
            MediaPlayer.Pause();
            MediaPlayer.SpeedRatio = value;
            MediaPlayer.Play();
            Set(ref _speedRatio, value);
        }
    }

    #region Play & Pause обёртки
    protected void PlayAudio()
    {
        MediaPlayer.Play();
        IsPlaying = true;
    }

    protected void PlayAudio(AudioFile audio)
    {
        MediaPlayer.Open(audio.Path);
        MediaPlayer.Play();
        IsPlaying = true;
    }

    protected void PauseAudio()
    {
        MediaPlayer.Pause();
        IsPlaying = false;
    }
    #endregion
}

