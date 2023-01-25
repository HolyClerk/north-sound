using NorthSound.Infrastructure.Commands.Base;

namespace NorthSound.Client.ViewModels.Interfaces;

internal interface ISongViewModel
{
    public RelayCommand PlayCommand { get; }
}
