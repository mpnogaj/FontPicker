using Avalonia.Media;

namespace AvaloniaFontPicker
{
    public class Font
    {
        public FontFamily FontFamily { get; init; } = FontFamily.Default;
        public FontWeight FontWeight { get; init; } = FontWeight.Normal;
        public FontStyle FontStyle { get; init; } = FontStyle.Normal;
        public double FontSize { get; init; } = 12.0;
        public SolidColorBrush Foreground { get; init; } = new(Colors.Black);
    }
}