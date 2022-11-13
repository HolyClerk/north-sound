using NorthSound.Client.Infrastructure.Commands.Base;
using NorthSound.Client.Services;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;

namespace NorthSound.Client.ViewModels;

internal class SongViewModel : ViewModelBase
{
    private int _duration;
    private double _volume;

    private Song? _currentSong;
    private MediaPlayerShell _mediaPlayer;
    private RelayCommand? _playCommand;

    public SongViewModel() => _mediaPlayer = new MediaPlayerShell();

    public RelayCommand PlayCommand
    {
        get
        {
            return _playCommand ??= new RelayCommand(execute =>
            {
                if (!_mediaPlayer.IsPlaying && SelectedSong != null)
                {
                    _mediaPlayer.Play();
                    return;
                }

                _mediaPlayer.Pause();
            }, canExecute =>
            {
                return SelectedSong != null;
            });
        }
    }

    public Song? SelectedSong
    {
        get => _currentSong;
        set
        {
            Set(ref _currentSong, value);
            _mediaPlayer.CurrentSong = value;
            _mediaPlayer.Play();
        }
    }

    public double SongVolume
    {
        get => _volume;
        set
        {
            Set(ref _volume, value);
            _mediaPlayer.SetVolume(value);
        }
    }

    public int Duration
    {
        get => _duration;
        set
        {
            _duration = _mediaPlayer.GetDuration();
            Set(ref _duration, value);
        }
    }
}

