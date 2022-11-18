using NorthSound.Client.Infrastructure.Commands.Base;
using NorthSound.Client.Services;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using System.Windows.Threading;
using System;
using System.Threading.Tasks;

namespace NorthSound.Client.ViewModels;

internal class SongViewModel : ViewModelBase
{
    private double _duration;
    private double _volume;
    private double _songPosition;

    private Song? _selectedSong;
    private MediaPlayerShell _mediaPlayer;
    private RelayCommand? _playCommand;

    public SongViewModel()
    {
        var dispatcherTimer = new DispatcherTimer();

        _mediaPlayer = new MediaPlayerShell();
        _mediaPlayer.SongStarted += (s, e) =>
        {
            Duration = _mediaPlayer.GetDuration();
            SongVolume = _mediaPlayer.GetVolume();
        };

        dispatcherTimer.Tick += (s, e) => SongPosition = _mediaPlayer.GetPosition();

        dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10); // 10ms
        dispatcherTimer.Start();
    }

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
        get => _selectedSong;
        set
        {
            if (value == null)
            {
                return;
            }

            Set(ref _selectedSong, value);
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

    public double Duration
    {
        get => _duration;
        set
        {
            Set(ref _duration, value);
        }
    }

    public double SongPosition
    {
        get => _songPosition;
        set
        {
            Set(ref _songPosition, value);
            _mediaPlayer.SetPosition(value);
        }
    }
}

