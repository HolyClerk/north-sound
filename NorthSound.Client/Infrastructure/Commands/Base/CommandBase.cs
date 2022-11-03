using System;
using System.Windows.Input;

namespace NorthSound.Client.Infrastructure.Commands.Base;

public abstract class CommandBase : ICommand
{
    /// <summary>
    /// Вызывается при изменении возвращаемого значения
    /// метода CanExecute
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>
    /// Метод проверяющий возможность исполнения команды.
    /// Если метод возвращает <see langword="false"/> - элемент 
    /// управления, вызывавший команду, станет недоступен.
    /// </summary>
    /// <param name="parameter"></param>
    public abstract bool CanExecute(object? parameter);

    /// <summary>
    /// Метод, который будет вызван при срабатывании команды.
    /// </summary>
    /// <param name="parameter">Параметры команды</param>
    public abstract void Execute(object? parameter);
}