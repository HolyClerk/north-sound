using NorthSound.Client.Infrastructure.Commands.Base;
using System.Windows;

namespace NorthSound.Client.Infrastructure.Commands;

internal class CloseApplication : CommandBase
{
    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter) => Application.Current.Shutdown();
}
