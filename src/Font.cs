using Avalonia.Media;

namespace AvaloniaFontPicker
{
    public class Font
    {
        public FontFamily FontFamily { get; set; } = FontFamily.Default;
        public FontWeight FontWeight { get; set; } = FontWeight.Normal;
        public FontStyle FontStyle { get; set; } = FontStyle.Normal;
        public double FontSize { get; set; } = 12.0;
        public SolidColorBrush Foreground { get; set; } = new(Colors.Black);
    }
}