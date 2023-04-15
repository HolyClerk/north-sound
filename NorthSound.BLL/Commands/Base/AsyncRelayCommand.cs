using System;
using System.Threading.Tasks;

namespace NorthSound.BLL.Commands.Base;

public class AsyncRelayCommand : CommandBase
{
    private readonly Func<object?, Task> _execute;
    private readonly Func<object, bool>? _canExecute;

    public AsyncRelayCommand(Func<object?, Task> execute, Func<object, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter) 
        => _canExecute?.Invoke(parameter!) ?? true;

    public override async void Execute(object? parameter) 
        => await _execute(parameter);
}