using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Skia;
using Avalonia.Visuals.Media;
using SkiaSharp;

namespace AvaloniaFontPicker
{
    public class MainDialog : Window, INotifyPropertyChanged
    {
        private Font _currentFont;
        private List<string> _installedFonts;
        private List<string> _availableStyles;
        private string _selectedFontFamily;
        
        public MainDialog() : this(null) { }

        public MainDialog(Font? font)
        {
            InitializeComponent();
            _currentFont = font ?? new Font();
            _installedFonts = FontManager.Current.GetInstalledFontFamilyNames().ToList();
            _selectedFontFamily = FontFamily.Default.Name;
            var list = SKFontManager.Default.GetFontStyles("Arial");
            _availableStyles = new List<string>();
            foreach (var skFontStyle in list)
            {
                var skTypeface = list.CreateTypeface(skFontStyle);
                var typeface = new Typeface(skTypeface.FamilyName, skTypeface.FontSlant.ToAvalonia(), (FontWeight) skTypeface.FontWeight);
                _availableStyles.Add(TypefaceToString(typeface));
            }
            DataContext = this;
        }

        public Font CurrentFont
        {
            get => _currentFont;
            set
            {
                _currentFont = value;
                OnPropertyChanged();
            }
        }

        public List<string> InstalledFonts
        {
            get => _installedFonts;
            set
            {
                _installedFonts = value;
                OnPropertyChanged();
            }
        }

        public List<string> AvailableStyles
        {
            get => _availableStyles;
            set
            {
                _availableStyles = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFontFamily
        {
            get => _selectedFontFamily;
            set
            {
                _selectedFontFamily = value;
                OnPropertyChanged();
                //Update font styles
                var list = SKFontManager.Default.GetFontStyles(value);
                var availableStyles = new List<string>();
                foreach (var skFontStyle in list)
                {
                    var skTypeface = list.CreateTypeface(skFontStyle);
                    var typeface = new Typeface(skTypeface.FamilyName, skTypeface.FontSlant.ToAvalonia(), (FontWeight) skTypeface.FontWeight);
                    availableStyles.Add(TypefaceToString(typeface));
                }
                AvailableStyles = availableStyles;
            }
        }

        private string TypefaceToString(Typeface t)
        {
            return $"{t.Weight}-{t.Style}";
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