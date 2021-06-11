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
            if (values != null && values.Count() == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (values[i].GetType() != typeof(byte))
                    {
                        return AvaloniaProperty.UnsetValue;
                    }
                }

                var color = Color.FromArgb(
                    (byte)values[0],
                    (byte)values[1],
                    (byte)values[2],
                    (byte)values[3]);
                
                if(color != Color.FromArgb(0, 0, 0, 0)) Console.WriteLine("a");

                return new SolidColorBrush(color);
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}