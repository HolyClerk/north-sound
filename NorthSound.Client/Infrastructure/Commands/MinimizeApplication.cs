using NorthSound.Client.Infrastructure.Commands.Base;
using System.Windows;

namespace NorthSound.Client.Infrastructure.Commands;

internal class MinimizeApplication : CommandBase
{
    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter) => Application.Current.MainWindow.WindowState = WindowState.Minimized;
}
