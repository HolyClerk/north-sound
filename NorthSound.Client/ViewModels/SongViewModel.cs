using NorthSound.Client.Infrastructure.Commands.Base;
using NorthSound.Client.Services;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using System.Windows.Media;

namespace NorthSound.Client.ViewModels;

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
                    PlaySong();
                    return;
                }

                PauseSong();
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

            PlaySong(value);
            Set(ref _selectedSong, value);
        }
    }
}