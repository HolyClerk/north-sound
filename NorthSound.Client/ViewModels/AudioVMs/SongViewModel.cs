using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;

namespace NorthSound.Client.ViewModels.AudioVMs;

internal class SongViewModel : PlayerVmBase
{
    private Song? _selectedSong;
    private RelayCommand? _playCommand;

    public SongViewModel() : base() { }

    public RelayCommand PlayCommand
    {
        get
        {
            return _playCommand ??= new RelayCommand(execute =>
            {
                if (!IsPlaying && SelectedSong != null)
                {
                    PlayAudio();
                    return;
                }

                PauseAudio();
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

            PlayAudio(value);
            Set(ref _selectedSong, value);
        }
    }
}