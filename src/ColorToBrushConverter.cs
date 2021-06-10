using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace AvaloniaFontPicker
{
    public class ColorToBrushConverter : IMultiValueConverter
    {
        public object Convert(IList<object>? values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Count() != 3) return AvaloniaProperty.UnsetValue;
            for (var i = 0; i < 3; i++)
            {
                if (values[i].GetType() != typeof(byte))
                {
                    return AvaloniaProperty.UnsetValue;
                }
            }
            
            if((byte)values[0] != 0 ||
            (byte)values[1] != 0 ||
            (byte)values[2] != 0)
                Console.WriteLine("a");
            
            var color = Color.FromRgb(
                (byte)values[0],
                (byte)values[1],
                (byte)values[2]);

            return new SolidColorBrush(color);
        }
    }
}