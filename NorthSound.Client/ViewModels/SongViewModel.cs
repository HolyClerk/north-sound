using NorthSound.Client.ViewModels.Base;
using NorthSound.Client.ViewModels.Interfaces;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal class SongViewModel : ViewModelBase, ISongViewModel
{
    public SongViewModel() : base() 
    {
        AudioCollection = new ObservableCollection<Song>()
        {
            new Song()
            {
                Id = 1,
                Author = "tEST",
                Name = "ttt"
            }
        };
    }

    private ObservableCollection<Song> _audioCollection = null!;
    public ObservableCollection<Song> AudioCollection 
    {
        get => _audioCollection;
        set => Set(ref _audioCollection, value);
    }

    private Song? _selectedSong;
    public Song? SelectedSong
    {
        get => _selectedSong;
        set => Set(ref _selectedSong, value);
    }

    private RelayCommand? _playCommand;
    public RelayCommand PlayCommand
    {
        get
        {
            return _playCommand ??= new RelayCommand(execute =>
            {

            }, canExecute =>
            {
                return SelectedSong != null;
            });
        }
    }
}
