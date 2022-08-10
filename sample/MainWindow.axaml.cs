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
		private readonly TextBox _sampleBox;

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

		private async void Button_OnClick(object? sender, RoutedEventArgs e)
		{
			MainDialog fontDialog = new(new Font
			{
				FontFamily = _sampleBox.FontFamily,
				FontSize = _sampleBox.FontSize,
				FontStyle = _sampleBox.FontStyle,
				FontWeight = _sampleBox.FontWeight,
				Foreground = (SolidColorBrush)_sampleBox.Foreground!
			});
			await fontDialog.ShowDialog(this);
		}
	}
}