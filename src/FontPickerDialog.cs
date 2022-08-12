using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using AvaloniaFontPicker.Properties;

namespace AvaloniaFontPicker
{
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
				Title = this.Title,
				WindowStartupLocation = this.StartupLocation
			};
			return dialog.ShowDialog<Font?>(owner);
		}
	}
}
