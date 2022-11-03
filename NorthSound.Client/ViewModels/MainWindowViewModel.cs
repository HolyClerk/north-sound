using NorthSound.Client.ViewModels.Base;

namespace NorthSound.Client.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private string _title = "";

    public MainWindowViewModel()
    {
        Title = "North Sound - Главная страница";
    }

    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }
}

