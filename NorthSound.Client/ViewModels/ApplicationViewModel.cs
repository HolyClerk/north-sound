using NorthSound.Client.ViewModels.Base;

namespace NorthSound.Client.ViewModels;

internal class ApplicationViewModel : ViewModelBase
{
    public ApplicationViewModel(SongViewModel songVm)
    {
        SongVm = songVm;
    }

    private string _title = "";
    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    public SongViewModel SongVm { get; }
}