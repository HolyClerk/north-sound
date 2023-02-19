using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.AudioPlayer.Base;
using System.Windows;
using System.Windows.Media;

namespace NorthSound.Infrastructure.Services.AudioPlayer;

public class AudioPlayer : IPlayer
{
    private MediaPlayer _mediaPlayer;

	public AudioPlayer()
	{
        _mediaPlayer = new MediaPlayer();

        _mediaPlayer.MediaOpened += (o, e) 
            => _isPlaying = true;

        _mediaPlayer.MediaEnded += (o, e)
            => _isPlaying = false;
    }

    private bool _isPlaying;

    private RelayCommand _playCommand = null!;
    public RelayCommand PlayCommand
    {
        get
        {
            return _playCommand ??= new RelayCommand(obj =>
            {
                if (obj is Song song)
                    ExecutePlayCommand(song);
            }, selectedObject => selectedObject as Song is not null);
        }
    }

    private RelayCommand _pauseCommand = null!;
    public RelayCommand PauseCommand
    {
        get
        {
            return _pauseCommand ??= new RelayCommand(
                obj => ExecutePauseCommand(), 
                selectedObject => _mediaPlayer.CanPause);
        }
    }

    private RelayCommand _stopCommand = null!;
    public RelayCommand StopCommand
    {
        get
        {
            return _stopCommand ??= new RelayCommand(
                obj => _mediaPlayer.Stop(),
                selectedObject => _mediaPlayer.CanFreeze);
        }
    }

    private void ExecutePlayCommand(Song song)
    {
        if (song is null)
            return;

        var currentPath = _mediaPlayer.Source?.AbsolutePath;
        var songPath = song.Path.AbsolutePath;

        if (_isPlaying 
            && currentPath == songPath)
        {
            ExecutePauseCommand();
            return;
        }

        if (_mediaPlayer.Source is null 
            || (_mediaPlayer.Source is not null && currentPath != songPath))
        {
            _mediaPlayer.Open(song.Path);
        }

        Play();
    }

    private void OpenPlay(Song song)
    {
        _mediaPlayer.Open(song.Path); // _isPlaying = true
        _mediaPlayer.Play();
        _isPlaying = true;
    }

    private void Play()
    {
        _mediaPlayer.Play();
        _isPlaying = true;
    }

    private void ExecutePauseCommand()
    {
        _mediaPlayer.Pause();
        _isPlaying = false;
    }
}
