using System;
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
        private Font _currentFont = new();
        private List<string> _installedFonts = new();
        private List<string> _availableStyles = new();
        private readonly List<string> _fontSizes = new() {"8", "9", "10", "11", "12", "13", "14", "16", "18", "20", "24", "26", "28", "36", "48", "72"};
        private string _selectedFontFamily = string.Empty;
        private string _selectedFontSize = string.Empty;
        
        public MainDialog() : this(null) { }

        public MainDialog(Font? font)
        {
            CurrentFont = font ?? new Font();
            InstalledFonts = FontManager.Current.GetInstalledFontFamilyNames().ToList();
            // Default font is always installed so it can't be null
            SelectedFontFamily = InstalledFonts.Find(x => x == FontManager.Current.DefaultFontFamilyName)!;
            UpdateTypefaces(SelectedFontFamily);
            SelectedFontSize = _fontSizes[4];
            InitializeComponent();
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
                UpdateTypefaces(value);
            }
        }

        public List<string> FontSizes
        {
            get => _fontSizes;
        }

        public string SelectedFontSize
        {
            get => _selectedFontSize;
            set
            {
                _selectedFontSize = value;
                OnPropertyChanged(value);
            }
        }

        private void UpdateTypefaces(string font)
        {
            var list = SKFontManager.Default.GetFontStyles(font);
            var availableStyles = new List<string>();
            foreach (var skFontStyle in list)
            {
                var skTypeface = list.CreateTypeface(skFontStyle);
                var fontWeightNumber = skTypeface.FontWeight;
                if(!Enum.IsDefined(typeof(FontWeight), fontWeightNumber)) RoundToHundreds(ref fontWeightNumber);
                var fontWeight = (FontWeight) fontWeightNumber;
                var typeface = new Typeface(skTypeface.FamilyName, skTypeface.FontSlant.ToAvalonia(), fontWeight);
                availableStyles.Add(TypefaceToString(typeface));
            }

            AvailableStyles = availableStyles;
        }

        private void RoundToHundreds(ref int number)
        {
            // 250 -> 300
            // 230 -> 200
            // 270 -> 300
            if ((number / 10) % 10 >= 5) number = ((number / 10) + 1) * 10;
            else number = (number / 10) * 10;
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