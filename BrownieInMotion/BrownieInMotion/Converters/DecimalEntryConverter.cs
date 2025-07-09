#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
using System.Globalization;

namespace BrownieInMotion.Converters;

public class DecimalEntryConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Exibe sempre com ponto ou vírgula, conforme cultura
        if (value is double d)
            return d.ToString("G", culture);
        if (value is null)
            return string.Empty;
        return value.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var str = value?.ToString()?.Replace(',', '.') ?? string.Empty;
        if (double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;
        return 0.0;
    }
}