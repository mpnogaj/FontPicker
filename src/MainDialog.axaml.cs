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
                OnPropertyChanged();
            }
        }
        
        private byte _r = byte.MaxValue;
        private byte _g = 13;
        private byte _b = 8;
        
        public string Hex
        {
            get => ToHexString();
            set
            {
                try
                {
                    Console.WriteLine($"Ustawiam kolor na: {value}");
                    FromHexString(value, out _r, out _g, out _b);
                    OnPropertyChanged(nameof(R));
                    OnPropertyChanged(nameof(G));
                    OnPropertyChanged(nameof(B));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public byte R
        {
            get => _r;
            set
            {
                if (_r != value)
                {
                    _r = value;
                    OnPropertyChanged(nameof(R));
                }
                OnPropertyChanged(nameof(Hex));
            }
        }
        
        public byte G
        {
            get => _g;
            set
            {
                if (_g != value)
                {
                    _g = value;
                    OnPropertyChanged(nameof(G));
                }
                OnPropertyChanged(nameof(Hex));
            }
        }
        
        public byte B
        {
            get => _b;
            set
            {
                if (_b != value)
                {
                    _b = value;
                    OnPropertyChanged(nameof(B));
                }
                OnPropertyChanged(nameof(Hex));
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

        private void FromHexString(string value, out byte r, out byte g, out byte b)
        {
            var color = Color.Parse(value);
            //a = color.A;
            r = color.R;
            g = color.G;
            b = color.B;
        }
        
        private string ToHexString()
        {
            return $"#{ToUint32():X8}";
        }
        
        private uint ToUint32()
        {
            return ((uint)R << 16) | ((uint)G << 8) | (uint)B;
        }

        public new event PropertyChangedEventHandler? PropertyChanged;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}