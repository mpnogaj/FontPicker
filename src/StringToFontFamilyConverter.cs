using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace AvaloniaFontPicker
{
	internal class StringToFontFamilyConverter : IValueConverter
	{
		public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
			=> new FontFamily((string)(value ?? throw new ArgumentNullException(nameof(value))));

		public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
			=> ((FontFamily)(value ?? throw new ArgumentNullException(nameof(value)))).Name;
	}
}
