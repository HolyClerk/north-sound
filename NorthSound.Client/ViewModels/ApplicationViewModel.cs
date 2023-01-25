using NorthSound.Client.ViewModels.Base;
using NorthSound.Client.ViewModels.Interfaces;
using NorthSound.Domain.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace NorthSound.Client.ViewModels;

internal class ApplicationViewModel : ViewModelBase
{
    private string _title = "";
    private TabItem? _selectedTabItem;

    public ApplicationViewModel(ISongViewModel songViewModel)
    {
        SongVm = songViewModel;
        /*SongVm.AudioCollection = new ObservableCollection<Song>()
        {
            new Song()
            {
                Id = 1,
                Author = "tEST",
                Name = "ttt"
            }
        };*/

        // PlaylistViewModel.UpdateAudio(IService);
    }

    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    public TabItem? SelectedTabItem
    {
        get => _selectedTabItem;
        set
        {
            Set(ref _selectedTabItem, value);
            Title = $"North Sound - {value?.Name}";
        }
    }

    public ISongViewModel SongVm { get; set; }
}

