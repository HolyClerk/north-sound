using NorthSound.Infrastructure.Commands.Base;

namespace NorthSound.Infrastructure.Services.AudioPlayer.Base;

public interface IPlayer
{
    public RelayCommand PlayCommand { get; }
    public RelayCommand PauseCommand { get; }
    public RelayCommand StopCommand { get; }
}
