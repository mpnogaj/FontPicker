using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Skia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace AvaloniaFontPicker
{
    internal sealed class Dialog : Window, INotifyPropertyChanged
    {
        private List<FontWeightStyle> _availableStyles = new();
        private List<string> _installedFonts = new();
        private string _selectedFontFamily = string.Empty;
        private string _selectedFontSize = string.Empty;
        private FontWeightStyle _selectedFontWeightStyle = new(FontWeight.Normal, FontStyle.Normal);
        private Color _selectedForeground;

		//For designer
		[UsedImplicitly]
        public Dialog() : this("The quick brown fox jumps over the lazy dog")
        {

        }

        public Dialog(string showcaseString, Font? font = null)
        {
            InitializeComponent();
            ShowcaseString = showcaseString;
            CurrentFont = font ?? new Font();
            InstalledFonts = FontManager.Current.GetInstalledFontFamilyNames().OrderBy(x => x).ToList();
            SelectedForeground = CurrentFont.Foreground.Color;
            if (CurrentFont != new Font())
            {
                SelectedFontFamily = InstalledFonts.Find(x => x == CurrentFont.FontFamily.Name) ??
                                     InstalledFonts.Find(x => x == FontManager.Current.DefaultFontFamilyName)!;
                SelectedFontWeightStyle = new FontWeightStyle(CurrentFont.FontWeight, CurrentFont.FontStyle);
                UpdateFontWeightStyles(SelectedFontFamily, SelectedFontWeightStyle);
                SelectedFontSize = ((int)font!.FontSize).ToString();
            }
            else
            {
                // Default font is always installed so it can't be null
                SelectedFontFamily = InstalledFonts.Find(x => x == FontManager.Current.DefaultFontFamilyName)!;
                UpdateFontWeightStyles(SelectedFontFamily, new FontWeightStyle(FontWeight.Normal, FontStyle.Normal));
                SelectedFontSize = FontSizes[4];
            }

            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private Font CurrentFont { get; }

        internal List<string> InstalledFonts
        {
            get => _installedFonts;
            set => SetValue(value, ref _installedFonts);
        }

        internal List<FontWeightStyle> AvailableStyles
        {
            get => _availableStyles;
            set => SetValue(value, ref _availableStyles);
        }

        public static List<string> FontSizes =>
            new() { "8", "9", "10", "11", "12", "13", "14", "16", "18", "20", "24", "26", "28", "36", "48", "72" };


        internal string SelectedFontFamily
        {
            get => _selectedFontFamily;
            set
            {
                SetValue(value, ref _selectedFontFamily);
                UpdateFontWeightStyles(value, SelectedFontWeightStyle);
            }
        }

        internal string SelectedFontSize
        {
            get => _selectedFontSize;
            set => SetValue(value, ref _selectedFontSize);
        }

        internal FontWeightStyle SelectedFontWeightStyle
        {
            get => _selectedFontWeightStyle;
            set => SetValue(value, ref _selectedFontWeightStyle);
        }

        internal Color SelectedForeground
        {
            get => _selectedForeground;
            set => SetValue(value, ref _selectedForeground);
        }

        internal string ShowcaseString { get; }

        internal RelayCommand OkCommand => new(() => this.Close(new Font
        {
            FontFamily = new FontFamily(SelectedFontFamily),
            FontSize = Convert.ToDouble(SelectedFontSize),
            FontStyle = SelectedFontWeightStyle.FontStyle,
            FontWeight = SelectedFontWeightStyle.FontWeight,
            Foreground = new SolidColorBrush(SelectedForeground)
        }));

        internal RelayCommand CancelCommand => new(this.Close);

        private void UpdateFontWeightStyles(string font, FontWeightStyle currentFontWeightStyle)
        {
            var list = SKFontManager.Default.GetFontStyles(font);
            var availableStyles = new SortedSet<FontWeightStyle>();
            foreach (var skFontStyle in list)
            {
                var skTypeface = list.CreateTypeface(skFontStyle);
                var fontWeightNumber = skTypeface.FontWeight;
                if (!Enum.IsDefined(typeof(FontWeight), fontWeightNumber))
                {
                    RoundToHundreds(ref fontWeightNumber);
                }
                var fontWeight = (FontWeight)fontWeightNumber;
                var typeface = new Typeface(skTypeface.FamilyName, skTypeface.FontSlant.ToAvalonia(), fontWeight);
                availableStyles.Add(new FontWeightStyle(typeface.Weight, typeface.Style));
            }
            AvailableStyles = availableStyles.ToList();
            SelectedFontWeightStyle = availableStyles.Contains(currentFontWeightStyle)
                ? currentFontWeightStyle
                : availableStyles.First();
        }

        private static void RoundToHundreds(ref int number)
        {
            number = (number % 100) >= 50 ? ((number / 10) + 1) * 10 : number / 10 * 10;
        }

        public new event PropertyChangedEventHandler? PropertyChanged;

        private void SetValue<T>(T value, ref T storage, [CallerMemberName] string? propertyName = null)
        {
            if (storage?.Equals(value) ?? false)
            {
                return;
            }

            storage = value;
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? throw new ArgumentNullException(nameof(propertyName))));
        }
    }

    internal readonly struct FontWeightStyle : IComparable<FontWeightStyle>
    {
        public FontWeight FontWeight { get; }
        public FontStyle FontStyle { get; }

        public FontWeightStyle(FontWeight fontWeight, FontStyle fontStyle)
        {
            FontWeight = fontWeight;
            FontStyle = fontStyle;
        }

        public int CompareTo(FontWeightStyle other)
            => other.FontWeight != this.FontWeight
                ? this.FontWeight.CompareTo(other.FontWeight)
                : this.FontStyle.CompareTo(other.FontStyle);

        public override int GetHashCode()
            => HashCode.Combine(FontWeight, FontStyle);

        public override bool Equals(object? obj)
            => obj is FontWeightStyle rhs && Equals(rhs);

        private bool Equals(FontWeightStyle rhs)
            => rhs.FontWeight == this.FontWeight && rhs.FontStyle == this.FontStyle;

        public override string ToString()
            => $"{FontWeight.GetLocalized()} - {FontStyle.GetLocalized()}";
    }
}