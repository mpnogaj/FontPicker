using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace AvaloniaFontPicker
{
	internal class ColorToBrushConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			var color = (Color)(value ?? throw new ArgumentNullException(nameof(value)));
			return new SolidColorBrush(color);
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			var brush = (SolidColorBrush)(value ?? throw new ArgumentNullException(nameof(value)));
			return brush.Color;
		}
	}
}