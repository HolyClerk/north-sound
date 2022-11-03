using NorthSound.Client.Infrastructure.Commands.Base;
using System.Diagnostics;
using System.Windows;

namespace NorthSound.Client.Infrastructure.Commands;

internal class SwitchWindowState : CommandBase
{
    private bool _isMaximized = false;

    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        if (_isMaximized)
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }
        else
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        _isMaximized = !_isMaximized;
    }
}