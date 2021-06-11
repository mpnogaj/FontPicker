using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AvaloniaFontPicker;

namespace AvaloniaFontPickerSample
{
    public partial class MainWindow : Window
    {
        private TextBox _sampleBox;
        
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _sampleBox = this.Find<TextBox>("SampleBox");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            FontDialog a = new(new Font
            {
                FontFamily = _sampleBox.FontFamily,
                FontSize = _sampleBox.FontSize,
                FontStyle = _sampleBox.FontStyle,
                FontWeight  = _sampleBox.FontWeight,
                Foreground = (SolidColorBrush) _sampleBox.Foreground
            });
            a.Show(this, (font) =>
            {
                _sampleBox.FontFamily = font.FontFamily;
                _sampleBox.FontStyle = font.FontStyle;
                _sampleBox.FontWeight = font.FontWeight;
                _sampleBox.FontSize = font.FontSize;
                _sampleBox.Foreground = font.Foreground;
            });
        }
    }
}