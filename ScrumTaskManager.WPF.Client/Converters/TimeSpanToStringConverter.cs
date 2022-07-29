using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;

namespace ScrumTaskManager.WPF.Client.Converters;
[ValueConversion(typeof(TimeSpan), typeof(String))]
public class TimeSpanToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((TimeSpan)value).ToString("hh':'mm':'ss");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var timeStr = (string) value;
        var parts = timeStr.Split(':');
        var correctParts = new List<string>();
        foreach (var part in parts)
        {
            if (part.Length == 1)
                correctParts.Add($"0{part}");
            else if(part.Length == 2)
                correctParts.Add(part);
            else correctParts.Add(part.Remove(2));
        }
        return TimeSpan.ParseExact(string.Join(":", correctParts), "hh':'mm':'ss", culture);
    }
}
