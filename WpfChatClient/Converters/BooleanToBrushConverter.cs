using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfChatClient.Converters;

public class BooleanToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isOwn && isOwn)
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2B6CB0")); // Darker blue for own
        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#44000000")); // Glassy black for others
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
