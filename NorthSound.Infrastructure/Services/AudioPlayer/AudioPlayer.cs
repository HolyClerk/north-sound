using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.AudioPlayer.Base;
using System.Windows.Media;

namespace NorthSound.Infrastructure.Services.AudioPlayer;

public class AudioPlayer : IPlayer
{
    private MediaPlayer _mediaPlayer;

	public AudioPlayer()
	{
        _mediaPlayer = new MediaPlayer();
    }

    private RelayCommand _playCommand = null!;
    public RelayCommand PlayCommand
    {
        get
        {
            return _playCommand ??= new RelayCommand(obj =>
            {
                var song = obj as Song;
                if (song is null)
                    return;

                _mediaPlayer.Open(song.Path);
                _mediaPlayer.Play();
            }, selectedObject => selectedObject as Song is not null);
        }
    }

    private RelayCommand _pauseCommand = null!;
    public RelayCommand PauseCommand
    {
        get
        {
            return _pauseCommand ??= new RelayCommand(
                obj => _mediaPlayer.Pause(), 
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

}
