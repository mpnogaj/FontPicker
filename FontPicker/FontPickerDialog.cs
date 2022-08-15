using Avalonia.Controls;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FontPicker.Properties;
using JetBrains.Annotations;

namespace FontPicker;

[PublicAPI]
public class FontPickerDialog
{
	public FontPickerDialog() : this(Thread.CurrentThread.CurrentCulture)
	{

	}

	public FontPickerDialog(CultureInfo cultureInfo)
	{
		Resources.Culture = cultureInfo;
		Title = Resources.DialogTitle;
		ShowcaseString = Resources.ShowcaseString;
	}

	public string Title { get; set; }
	public string ShowcaseString { get; set; }
	public WindowStartupLocation StartupLocation { get; set; }

	public Task<Font?> OpenDialog(Window owner, Font? defaultFont = null)
	{
		var dialog = new Dialog(ShowcaseString, defaultFont)
		{
			Title = Title,
			WindowStartupLocation = StartupLocation
		};
		return dialog.ShowDialog<Font?>(owner);
	}
}