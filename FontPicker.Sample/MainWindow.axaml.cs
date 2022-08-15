using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace FontPicker.Sample
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

		private Font CurrentFont
		{
			get => new()
			{
				FontFamily = _sampleBox.FontFamily,
				FontSize = _sampleBox.FontSize,
				FontStyle = _sampleBox.FontStyle,
				FontWeight = _sampleBox.FontWeight,
				Foreground = (SolidColorBrush)_sampleBox.Foreground!
			};
			set
			{
				_sampleBox.FontFamily = value.FontFamily;
				_sampleBox.FontSize = value.FontSize;
				_sampleBox.FontStyle = value.FontStyle;
				_sampleBox.FontWeight = value.FontWeight;
				_sampleBox.Foreground = value.Foreground;
			}
		}

		private async void Button_OnClick(object? sender, RoutedEventArgs e)
		{
			var dialog = new FontPickerDialog()
			{
				StartupLocation = WindowStartupLocation.CenterOwner
			};
			var res = await dialog.OpenDialog(this, CurrentFont);
			if (res != null)
			{
				CurrentFont = res;
			}
		}
	}
}