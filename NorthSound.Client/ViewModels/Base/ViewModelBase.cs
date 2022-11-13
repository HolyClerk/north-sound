using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NorthSound.Client.ViewModels.Base;

/// <summary>
/// Шаблон базового класс для ViewModel'ей.
/// (Слой предметной области)
/// </summary>
internal abstract class ViewModelBase : INotifyPropertyChanged
{
    /// <summary>
    /// Событие изменения свойства.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Метод, вызывающий подписчиков события PropertyChanged.
    /// </summary>
    /// <param name="propertyName">Имя свойства.</param>
    protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Обновляет значение свойства и разрешает проблему 
    /// кольцевых изменений свойства.
    /// </summary>
    /// <typeparam name="T">Тип меняемого свойства</typeparam>
    /// <param name="field">Ссылка на поле(свойства)</param>
    /// <param name="value">Новое значение</param>
    /// <param name="propertyName">Имя вызывающего члена</param>
    /// <returns><see langword="true"/> если свойство не попало в кольцо и было изменено,
    /// в ином случае <see langword="false"/></returns>
    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(field, value))
        {
            return false;
        }

        field = value;
        RaisePropertyChanged(propertyName);

        return true;
    }
}

