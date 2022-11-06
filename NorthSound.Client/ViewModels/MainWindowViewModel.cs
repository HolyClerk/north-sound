using NorthSound.Client.ViewModels.Base;
using System.Windows.Controls;

namespace NorthSound.Client.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private string _title = "";
    private TabItem? _selectedTabItem;

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
}

