using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaFontPicker
{
    public class MainDialog : Window, INotifyPropertyChanged
    {
        private Font _currentFont;

        public MainDialog()
        {
            InitializeComponent();
            _currentFont = new Font();
            DataContext = this;
        }

        public MainDialog(Font font)
        {
            InitializeComponent();
            _currentFont = font;
            DataContext = this;
        }

        private Font CurrentFont
        {
            get => _currentFont;
            set
            {
                _currentFont = value;
                OnPropertyChanged(nameof(CurrentFont));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}