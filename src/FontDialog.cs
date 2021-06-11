using System;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace AvaloniaFontPicker
{
    public class FontDialog
    {
        private Font SelectedFont { get; set; }
        
        public FontDialog(Font? defaultFont = null)
        {
            SelectedFont = defaultFont ?? new Font();
        }

        public async Task Show(Window owner, Action<Font> callback, string title = "Select font")
        {
            var dialog = new MainDialog(SelectedFont);
            await dialog.ShowDialog(owner);
            if (dialog.ShouldSaveChanges) 
            { 
                SelectedFont = dialog.CurrentFont;
                callback(SelectedFont);
            }
        }
    }
}