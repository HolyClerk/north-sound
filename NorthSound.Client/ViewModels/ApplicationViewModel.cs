using Microsoft.Xaml.Behaviors.Core;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Client.ViewModels.Interfaces;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Base;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NorthSound.Client.ViewModels;

internal class ApplicationViewModel : ViewModelBase
{
    public ApplicationViewModel(SongViewModel songVm)
    {
        SongVm = songVm;
    }

    private string _title = "Empty";
    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    public SongViewModel SongVm { get; }
}