using System.Threading.Tasks;
using Avalonia.Controls;

namespace AvaloniaFontPicker
{
    public class FontDialog
    {
        public Font SelectedFont { get; private set; }
        
        public FontDialog(Window owner, Font? defaultFont = null) : this(owner, "Select Font", defaultFont){}

        public FontDialog(Window owner, string dialogTitle , Font? defaultFont = null)
        {
            SelectedFont = defaultFont ?? new Font();
            CreateAndShowDialog(owner, dialogTitle).Wait();
        }

        private async Task CreateAndShowDialog(Window owner, string title)
        {
            var dialog = new MainDialog(SelectedFont);
            await dialog.ShowDialog(owner);
        }
    }
}