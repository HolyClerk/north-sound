using NorthSound.Client.ViewModels.AudioVMs;
using NorthSound.Client.ViewModels.Base;
using System.Windows.Controls;

namespace NorthSound.Client.ViewModels;

internal class MainWindowVM : ViewModelBase
{
    private string _title = "";
    private TabItem? _selectedTabItem;

    public MainWindowVM()
    {
        AudioPage = new PageViewModel();
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

    #region Вложенные ViewModel'и
    public PageViewModel AudioPage
    {
        get; 
    }
    #endregion
}

