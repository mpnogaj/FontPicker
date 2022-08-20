using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace FontPicker.Converters
{
	internal class ColorToHsvColorConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
			=> ((Color)(value ?? throw new ArgumentNullException(nameof(value)))).ToHsv();

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
			=> ((HsvColor)(value ?? throw new ArgumentNullException(nameof(value)))).ToRgb();
	}
}
