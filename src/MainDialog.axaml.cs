using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Skia;
using SkiaSharp;

namespace AvaloniaFontPicker
{
    public class MainDialog : Window, INotifyPropertyChanged
    {
        private readonly TextBox _previewBox;

        private byte _a = byte.MaxValue;
        private List<string> _availableStyles = new();
        private List<FontStyleWeight> _availableStylesObj = new();
        private byte _b;
        private Font _currentFont = new();
        private byte _g;
        private List<string> _installedFonts = new();
        private byte _r;
        private string _selectedFontFamily = string.Empty;
        private string _selectedFontSize = string.Empty;
        private int _selectedStyleIndex;

        public MainDialog() : this(null)
        {
        }

        public MainDialog(Font? font)
        {
            InitializeComponent();
            _previewBox = this.Find<TextBox>("PreviewBox")!;
            CurrentFont = font ?? new Font();
            InstalledFonts = FontManager.Current.GetInstalledFontFamilyNames().OrderBy(x => x).ToList();

            if (font != new Font())
            {
                SelectedFontFamily = InstalledFonts.Find(x => x == font!.FontFamily.Name) ??
                                     InstalledFonts.Find(x => x == FontManager.Current.DefaultFontFamilyName)!;
                UpdateTypefaces(SelectedFontFamily);
                SelectedFontSize = ((int)font!.FontSize).ToString();
                var fontStyleWeight = new FontStyleWeight(font!.FontStyle, font!.FontWeight);
                var index = _availableStylesObj.FindIndex((x) => x.Equals(fontStyleWeight));
                SelectedStyleIndex = index != -1 ? index : 0;
                R = font.Foreground.Color.R;
                G = font.Foreground.Color.G;
                B = font.Foreground.Color.B;
            }
            else
            {
                // Default font is always installed so it can't be null
                SelectedFontFamily = InstalledFonts.Find(x => x == FontManager.Current.DefaultFontFamilyName)!;
                UpdateTypefaces(SelectedFontFamily);
                SelectedFontSize = FontSizes[4];
            }

            DataContext = this;
        }

        public bool ShouldSaveChanges { get; private set; }

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
                _previewBox.FontFamily = new FontFamily(value);
                OnPropertyChanged();
                UpdateTypefaces(value);
            }
        }

        public List<string> FontSizes { get; } = new()
            {"8", "9", "10", "11", "12", "13", "14", "16", "18", "20", "24", "26", "28", "36", "48", "72"};

        public string SelectedFontSize
        {
            get => _selectedFontSize;
            set
            {
                _selectedFontSize = value;
                OnPropertyChanged();
            }
        }

        public int SelectedStyleIndex
        {
            get => _selectedStyleIndex;
            set
            {
                value = Math.Max(0, value);
                try
                {
                    _selectedStyleIndex = value;
                    _previewBox.FontStyle = _availableStylesObj[value].FontStyle;
                    _previewBox.FontWeight = _availableStylesObj[value].FontWeight;
                }
                catch(IndexOutOfRangeException)
				{
                    _selectedStyleIndex = 0;
                    _previewBox.FontStyle = _availableStylesObj[0].FontStyle;
                    _previewBox.FontWeight = _availableStylesObj[0].FontWeight;
                }
                OnPropertyChanged();
            }
        }

        public string Hex
        {
            get => ToHexString();
            set
            {
                try
                {
					FromHexString(value, out _a, out _r, out _g, out _b);
                    OnPropertyChanged(nameof(A));
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

        public byte A
        {
            get => _a;
            set
            {
                if (value != _a)
                {
                    _a = value;
                    OnPropertyChanged(nameof(A));
                }

                OnPropertyChanged(nameof(Hex));
            }
        }

        public byte R
        {
            get => _r;
            set
            {
                if (value != _r)
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
                if (value != _g)
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
                if (value != _b)
                {
                    _b = value;
                    OnPropertyChanged(nameof(B));
                }

                OnPropertyChanged(nameof(Hex));
            }
        }

        public new event PropertyChangedEventHandler? PropertyChanged;

        private void UpdateTypefaces(string font)
        {
            var list = SKFontManager.Default.GetFontStyles(font);
            var availableStyles = new List<string>();
            var availableStylesObj = new List<FontStyleWeight>();
            foreach (var skFontStyle in list)
            {
                var skTypeface = list.CreateTypeface(skFontStyle);
                var fontWeightNumber = skTypeface.FontWeight;
                if (!Enum.IsDefined(typeof(FontWeight), fontWeightNumber)) RoundToHundreds(ref fontWeightNumber);
                var fontWeight = (FontWeight) fontWeightNumber;
                var typeface = new Typeface(skTypeface.FamilyName, skTypeface.FontSlant.ToAvalonia(), fontWeight);
                availableStyles.Add(TypefaceToString(typeface));
                availableStylesObj.Add(new FontStyleWeight(typeface.Style, typeface.Weight));
            }
            
            AvailableStyles = availableStyles;
            _availableStylesObj = availableStylesObj;
            SelectedStyleIndex = 0;
        }

        private static void RoundToHundreds(ref int number)
        {
            // 250 -> 300
            // 230 -> 200
            // 270 -> 300
            if (number / 10 % 10 >= 5) number = (number / 10 + 1) * 10;
            else number = number / 10 * 10;
        }

        private static string TypefaceToString(Typeface t)
        {
            return $"{t.Weight}-{t.Style}";
        }

        private static void FromHexString(string value, out byte a, out byte r, out byte g, out byte b)
        {
            var color = Color.Parse(value);
            a = color.A;
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
            return ((uint) R << 16) | ((uint) G << 8) | B;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SaveChanges(object? sender, RoutedEventArgs e)
        {
            CurrentFont = new Font
            {
                FontFamily = new FontFamily(SelectedFontFamily),
                FontSize = Convert.ToDouble(SelectedFontSize),
                FontStyle = _availableStylesObj[_selectedStyleIndex].FontStyle,
                FontWeight = _availableStylesObj[_selectedStyleIndex].FontWeight,
                Foreground = new SolidColorBrush(Color.FromRgb(_r, _g, _b))
            };
            ShouldSaveChanges = true;
            Close();
        }

        private void Cancel(object? sender, RoutedEventArgs e)
        {
            ShouldSaveChanges = false;
            Close();
        }
    }

    internal readonly struct FontStyleWeight
    {
        public readonly FontStyle FontStyle;
        public readonly FontWeight FontWeight;

        public FontStyleWeight(FontStyle fontStyle, FontWeight fontWeight)
        {
            FontStyle = fontStyle;
            FontWeight = fontWeight;
        }
    }
}