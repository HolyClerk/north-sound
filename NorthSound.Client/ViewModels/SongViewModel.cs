using NorthSound.Client.Infrastructure.Commands.Base;
using NorthSound.Client.Services;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;

namespace NorthSound.Client.ViewModels;

internal class SongViewModel : ViewModelBase
{
    private Song? _selectedSong;
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
        get => _selectedSong;
        set
        {
            Set(ref _selectedSong, value);
            _mediaPlayer.CurrentSong = value;
        }
    }
}

