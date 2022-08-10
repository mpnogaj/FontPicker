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
		
        private List<string> _availableStyles = new();
        private List<FontStyleWeight> _availableStylesObj = new();
        private Font _currentFont = new();
        private List<string> _installedFonts = new();
        private string _selectedFontFamily = string.Empty;
        private string _selectedFontSize = string.Empty;
        private int _selectedStyleIndex;
        private Color _selectedForeground;

        public MainDialog() : this(null)
        {
        }

        public MainDialog(Font? font)
        {
            InitializeComponent();
            _previewBox = this.Find<TextBox>("PreviewBox")!;
            CurrentFont = font ?? new Font();
            InstalledFonts = FontManager.Current.GetInstalledFontFamilyNames().OrderBy(x => x).ToList();
            SelectedForeground = CurrentFont.Foreground.Color;
			if (CurrentFont != new Font())
            {
	            SelectedFontFamily = InstalledFonts.Find(x => x == font!.FontFamily.Name) ??
	                                 InstalledFonts.Find(x => x == FontManager.Current.DefaultFontFamilyName)!;
                UpdateTypefaces(SelectedFontFamily);
                SelectedFontSize = ((int)font!.FontSize).ToString();
                var fontStyleWeight = new FontStyleWeight(font!.FontStyle, font!.FontWeight);
                var index = _availableStylesObj.FindIndex((x) => x.Equals(fontStyleWeight));
                SelectedStyleIndex = index != -1 ? index : 0;
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
            set => SetValue(value, ref _currentFont);
        }

        public List<string> InstalledFonts
        {
            get => _installedFonts;
            set => SetValue(value, ref _installedFonts);
        }

        public List<string> AvailableStyles
        {
            get => _availableStyles;
            set => SetValue(value, ref _availableStyles);
        }

        public string SelectedFontFamily
        {
            get => _selectedFontFamily;
            set
            {
				SetValue(value, ref _selectedFontFamily);
                _previewBox.FontFamily = new FontFamily(value);
                UpdateTypefaces(value);
            }
        }

        internal static List<string> FontSizes =>
            new(){"8", "9", "10", "11", "12", "13", "14", "16", "18", "20", "24", "26", "28", "36", "48", "72"};

        public string SelectedFontSize
        {
            get => _selectedFontSize;
            set => SetValue(value, ref _selectedFontSize);
        }

        public int SelectedStyleIndex
        {
            get => _selectedStyleIndex;
            set
            {
                value = Math.Max(0, value);
                try
                {
                    _previewBox.FontStyle = _availableStylesObj[value].FontStyle;
                    _previewBox.FontWeight = _availableStylesObj[value].FontWeight;
                    SetValue(value, ref _selectedStyleIndex);
                }
                catch(IndexOutOfRangeException)
				{
                    _previewBox.FontStyle = _availableStylesObj[0].FontStyle;
                    _previewBox.FontWeight = _availableStylesObj[0].FontWeight;
                    SetValue(0, ref _selectedStyleIndex);
				}
            }
        }

        internal Color SelectedForeground
        {
	        get => _selectedForeground;
			set => SetValue(value, ref _selectedForeground);
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
	        number = (number % 100) >= 50 ? ((number / 10) + 1) * 10 : number / 10 * 10;
        }

        private static string TypefaceToString(Typeface t)
        {
            return $"{t.Weight}-{t.Style}";
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected virtual void SetValue<T>(T value, ref T storage, [CallerMemberName] string? propertyName = null)
        {
	        if (storage?.Equals(value) ?? false)
	        {
		        return;
	        }

	        storage = value;
	        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? throw new ArgumentNullException(nameof(propertyName))));
        }

        private void SaveChanges(object? sender, RoutedEventArgs e)
        {
            CurrentFont = new Font
            {
                FontFamily = new FontFamily(SelectedFontFamily),
                FontSize = Convert.ToDouble(SelectedFontSize),
                FontStyle = _availableStylesObj[_selectedStyleIndex].FontStyle,
                FontWeight = _availableStylesObj[_selectedStyleIndex].FontWeight,
                Foreground = new SolidColorBrush(SelectedForeground)
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