using System;
using System.Globalization;
using System.Windows.Data;

namespace NorthSound.Client.Converters;

public class SelectedItemConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        // Получаем значения из двух привязок
        var selectedVirtualModel1 = values[0];
        var selectedVirtualModel2 = values[1];

        // Возвращаем массив значений, который будет передан в две вьюмодели
        return new object[] { selectedVirtualModel1, selectedVirtualModel2 };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        // Конвертация обратно не требуется, возвращаем null
        return null;
    }
}
