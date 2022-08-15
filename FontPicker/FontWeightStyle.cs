using System;
using Avalonia.Media;

namespace FontPicker;

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
		=> other.FontWeight != FontWeight
			? FontWeight.CompareTo(other.FontWeight)
			: FontStyle.CompareTo(other.FontStyle);

	public override int GetHashCode()
		=> HashCode.Combine(FontWeight, FontStyle);

	public override bool Equals(object? obj)
		=> obj is FontWeightStyle rhs && Equals(rhs);

	private bool Equals(FontWeightStyle rhs)
		=> rhs.FontWeight == FontWeight && rhs.FontStyle == FontStyle;

	public override string ToString()
		=> $"{FontWeight.GetLocalized()} - {FontStyle.GetLocalized()}";
}