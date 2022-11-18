using NorthSound.Domain.Models.Base;
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
            AudioVolume = MediaPlayer.Volume;
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

